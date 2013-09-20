using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The entry pont of the formatting and the caret management.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void FormatAndManageCaret(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			try
			{
				resultingFormattedValue = unformattedValue;

				if (NeedStopProcessing(unformattedValue))
				{
					return;
				}

				var state = InitProcessingStates(
					unformattedValue,
					textBeforeChanging,
					lastCaretPosition,
					resultingFormattedValue,
					caretPosition);

				FormatAndManageCaretRaw(state);

				resultingFormattedValue = state.FormattedValue;
				caretPosition = state.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				// Let's set default states, since we screwed up.
				resultingFormattedValue = String.Empty;
				caretPosition = 0;
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
		private void FormatAndManageCaretRaw(FormatterState state)
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
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(FormatterState state)
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
		private string IntegerPartProcessingWithCaret(FormatterState state)
		{
			// TODO.it3xl.com: Remove the magic constants! Here and around.

			var number = state.FormattedValue.Split(DecimalSeparator);

			// Integral part processing.
			var integer = number.First();

			if (1 < integer.Length
				&& state.PreviousInteger == "0"
				&& integer.Last() == '0')
			{
				integer = integer.Remove(integer.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < integer.Length && integer.First() == '0')
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
				integer = "0";

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
		private string PartialPartProcessingWithCaret(FormatterState state, int integerLength)
		{
			var number = state.FormattedValue.Split(DecimalSeparator);
			// Processing of the fractional part of a number.
			var partial = number.Last();
			// After the decimal point should be two digits.
			if (partial.Length == 0)
			{
				partial = "00";
			}
			else if (partial.Length == 1)
			{
				partial += "0";
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
