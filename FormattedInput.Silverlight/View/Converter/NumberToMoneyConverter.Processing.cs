using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Helpful method for tests of the <see cref="FormatAndManageCaret"/> method.
		/// <seealso cref="FormatAndManageCaret"/>
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void TestFormatAndManageCaret(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			FocusEnum focusState,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			TextBeforeChanging = textBeforeChanging;
			CaretPositionBeforeTextChanging = lastCaretPosition;

			FormatAndManageCaret(unformattedValue, focusState, out resultingFormattedValue, ref caretPosition);
		}

		/// <summary>
		/// The entry pont of the formatting and the caret management.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void FormatAndManageCaret(
			String unformattedValue,
			FocusEnum focusState,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			var lastCaretPosition = CaretPositionBeforeTextChanging;

			resultingFormattedValue = unformattedValue;

			try
			{
				if (NeedStopProcessing(unformattedValue))
				{
					return;
				}

				var state = InitProcessingStates(
					unformattedValue,
					TextBeforeChanging,
					lastCaretPosition,
					focusState,
					caretPosition);

				FormatAndManageCaretRaw(state);

				resultingFormattedValue = state.FormattedValue;

				if (state.CaretPosition < 0)
				{
					// It's definetly an error.
					// Fix the impossible negative caret position for sake of the unwanted exception.
					state.CaretPosition = 0;
				}
				if (resultingFormattedValue.Length < state.CaretPosition)
				{
					// It's definetly an error.
					state.CaretPosition = resultingFormattedValue.Length;
				}

				caretPosition = state.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				// Let's set default states, since we screwed up.
				resultingFormattedValue = String.Empty;
				caretPosition = 0;
			}
			finally
			{
				TextBeforeChanging = resultingFormattedValue;
				CaretPositionBeforeTextChanging = caretPosition;
			}
		}

		private static bool NeedStopProcessing(string unformattedValue)
		{
			if (unformattedValue == String.Empty)
			{
				// The empty value is correct and will stay unprocessed.

				return true;
			}
			return false;
		}

		/// <summary>
		/// The lower implementation of a formatting.
		/// </summary>
		/// <param name="state"></param>
		private void FormatAndManageCaretRaw(ProcessingState state)
		{
			DecimalSeparatorAlternatingReplacing(state);
			DecimalSeparatorDeletedProcessingWithCaret(state);
			DecimalSeparatorMissed(state);
			DecimalSeparatorExcessiveProcessingWithCaret(state);

			GroupSeparatorProcessingWithCaret(state);

			NotDigitCharsProcessingWithCaret(state);

			var integer = IntegerPartProcessingWithCaret(state);

			var partial = PartialPartProcessingWithCaret(state, integer.Length);

			var preliminatyFormattedValue = integer + DecimalSeparator + partial;
			state.FormattedValue = FormatByPrecisionForDouble(preliminatyFormattedValue);

			if(state.JumpCaretToEndOfInteger)
			{
				state.CaretPosition = integer.Length;
			}
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(ProcessingState state)
		{
			var stringForIteraction = state.FormattedValue;
			foreach (var @char in stringForIteraction)
			{
				if (CustomSerialilzationChars.Contains(@char))
				{
					continue;
				}

				// Let's delete only the first entry of that char for now.
				var index = state.FormattedValue.IndexOf(@char);
				state.FormattedValue = state.FormattedValue.Remove(index, 1);

				if (index <= (state.CaretPosition - 1))
				{
					state.CaretPosition -= 1;
				}
			}
		}

		/// <summary>
		/// Processing of the integer part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private string IntegerPartProcessingWithCaret(ProcessingState state)
		{
			var number = state.FormattedValue.Split(DecimalSeparator);

			// Integral part processing.
			var integer = number.First();

			if (1 < integer.Length
				&& state.PreviousInteger == ZeroString
				&& integer.Last() == ZeroChar)
			{
				integer = integer.Remove(integer.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < integer.Length && integer.First() == ZeroChar)
			{
				integer = integer.Remove(0, 1);
				if (0 < state.CaretPosition)
				{
					state.CaretPosition--;
				}
			}

			// Before decimal separator should be the 0, if it's the first.
			if (integer.Length == 0)
			{
				integer = ZeroString;

				state.CaretPosition++;
			}

			if (GroupSeparator.IsDefault())
			{
				return integer;
			}

			// The group separator processing.
			var lengthWithoutSeparator = integer.Length;
			var caretPositionWithoutSeparator = state.CaretPosition;

			var integerInvert = String.Join(null, integer.Reverse());
			for (var i = 0; i < lengthWithoutSeparator; i++)
			{
				if (i == 0)
				{
					continue;
				}
				if (0 < i % 3)
				{
					continue;
				}

				var offset = i / 3 - 1;
				integerInvert = integerInvert.Insert(i + offset, GroupSeparatorChar);

				state.CaretPosition++;
			}

			integer = String.Join(null, integerInvert.Reverse());

			// Actualizes caret position.
			var digitsAfterCaretWithoutSeparator = Math.Abs(lengthWithoutSeparator - caretPositionWithoutSeparator);
			var separarotAmountAfterCaret = digitsAfterCaretWithoutSeparator / 3;
			if (0 < digitsAfterCaretWithoutSeparator && digitsAfterCaretWithoutSeparator % 3 == 0)
			{
				separarotAmountAfterCaret--;
			}
			if (state.DeletionType == DeletionDirection.BackspaceButton
				&& 0 < digitsAfterCaretWithoutSeparator
				&& digitsAfterCaretWithoutSeparator % 3 == 0)
			{
				separarotAmountAfterCaret++;
			}

			state.CaretPosition -= separarotAmountAfterCaret;

			return integer;
		}

		/// <summary>
		/// Processing of the partial part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="integerLength"></param>
		/// <returns></returns>
		private string PartialPartProcessingWithCaret(ProcessingState state, int integerLength)
		{
			var number = state.FormattedValue.Split(DecimalSeparator);
			// Processing of the fractional part of a number.
			var partial = number.Last();
			// After the decimal point should be two digits.
			if (partial.Length == 0)
			{
				partial = ZeroPartialString;
			}
			else if (partial.Length == 1)
			{
				partial += ZeroString;
			}
			else if (2 < partial.Length)
			{
				var positionAfterFirstPartialDigit = integerLength + 1 + partial.Length - 2 == state.CaretPosition;
				var needCutSecondPartialDigit = partial.Length == 3 && positionAfterFirstPartialDigit;
				if (needCutSecondPartialDigit)
				{
					partial = partial.Remove(1, 1);
				}
				else
				{
					partial = partial.Substring(0, 2);
				}
			}
			return partial;
		}

	}
}
