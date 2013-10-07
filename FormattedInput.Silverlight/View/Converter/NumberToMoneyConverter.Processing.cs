using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Controller;

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
				var state = new StateController(
						DecimalSeparator,
						PartialDisabled,
						GroupSeparator,
						TextBeforeChanging
					)
					.GetProcessingStates(
						lastCaretPosition,
						focusState,
						caretPosition,
						unformattedValue);

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

		/// <summary>
		/// The lower implementation of a formatting.
		/// </summary>
		/// <param name="state"></param>
		private void FormatAndManageCaretRaw(ProcessingState state)
		{
			if (SetCaretOnStart(state))
			{
				return;
			}

			DecimalSeparatorAlternatingReplacing(state);
			DecimalSeparatorDeletedProcessingWithCaret(state);

			// Catch a first digital input and ingnore others.
			if(0 < state.UnformattedValue.Length
				&& state.FormattedValue.Any(el => CustomSerialilzationChars.Contains(el)) == false)
			{
				state.FormattedValue = String.Empty;
				state.CaretPosition = 0;

				// !!!
				return;
			}

			DecimalSeparatorMissed(state);
			DecimalSeparatorExcessiveProcessingWithCaret(state);

			GroupSeparatorProcessingWithCaret(state);

			NotDigitCharsProcessingWithCaret(state);

			state.IntegerFormatted = state.GetInteger();
			state.PartialFormatted = state.GetPartial();

			IntegerPartProcessingWithCaret(state);
			PartialPartProcessingWithCaret(state);

			var preliminatyFormattedValue = state.IntegerFormatted + DecimalSeparator + state.PartialFormatted;
			state.FormattedValue = FormatByPrecisionForDouble(preliminatyFormattedValue);

			if(state.JumpCaretToEndOfInteger)
			{
				state.CaretPosition = state.IntegerFormatted.Length;
			}
		}

		/// <summary>
		/// Sets the caret position on the start.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private bool SetCaretOnStart(ProcessingState state)
		{
			if (state.FormattingType != FormattingAfter.CorrectValueResetting)
			{
				return false;
			}
			if (state.CaretPosition != 0)
			{
				return false;
			}

			state.CaretPosition = state.GetIntegerEndCaretPosition();

			return true;
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
		private void IntegerPartProcessingWithCaret(ProcessingState state)
		{
			// Deletion of the 0 in the integer part if a input started from the 0 value.
			if (1 < state.IntegerFormatted.Length
				&& state.IntegerPrevious == ZeroString
				&& state.IntegerFormatted.Last() == ZeroChar)
			{
				state.IntegerFormatted = state.IntegerFormatted.Remove(state.IntegerFormatted.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < state.IntegerFormatted.Length && state.IntegerFormatted.First() == ZeroChar)
			{
				state.IntegerFormatted = state.IntegerFormatted.Remove(0, 1);
				if (0 < state.CaretPosition)
				{
					state.CaretPosition--;
				}
			}

			// Before decimal separator should be the 0, if it's the first.
			if (state.IntegerFormatted.Length == 0)
			{
				state.IntegerFormatted = ZeroString;

				state.CaretPosition++;
			}

			if (GroupSeparator.IsDefault())
			{
				return;
			}

			// The group separator processing.
			var lengthWithoutSeparator = state.IntegerFormatted.Length;
			var caretPositionWithoutGroupSeparator = state.CaretPosition;

			var integerInvert = String.Join(null, state.IntegerFormatted.Reverse());
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

			state.IntegerFormatted = String.Join(null, integerInvert.Reverse());

			// Actualizes caret position.
			var digitsAfterCaretWithoutGroupSeparator = Math.Abs(lengthWithoutSeparator - caretPositionWithoutGroupSeparator);
			var separarotAmountAfterCaret = digitsAfterCaretWithoutGroupSeparator / 3;
			if (0 < digitsAfterCaretWithoutGroupSeparator && digitsAfterCaretWithoutGroupSeparator % 3 == 0)
			{
				separarotAmountAfterCaret--;
			}
			if (state.OneSymbolDeletionType == DeletionDirection.BackspaceButton
				&& 0 < digitsAfterCaretWithoutGroupSeparator
				&& digitsAfterCaretWithoutGroupSeparator % 3 == 0)
			{
				separarotAmountAfterCaret++;
			}

			state.CaretPosition -= separarotAmountAfterCaret;
		}

		/// <summary>
		/// Processing of the partial part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private void PartialPartProcessingWithCaret(ProcessingState state)
		{
			if(PartialDisabled)
			{
				return;
			}

			// After the decimal separator should be two digits for now (PartialBitness is not implemented).

			var decimalSeparatorPosition = 
				state.IntegerFormatted.InvokeNotNull(el => el.Length)
				+ DecimalSeparatorChar.Length;

			if (state.PartialFormatted.Length == 0)
			{
				state.PartialFormatted = ZeroPartialString;
			}
			else if (state.PartialFormatted.Length == 1)
			{
				if (state.CaretPosition == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.BackspaceButton)
				{
					state.PartialFormatted = ZeroString + state.PartialFormatted;
				}
				else if (state.CaretPosition == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.DeleteButton)
				{
					if(state.PartialPrevious.StartsWith(ZeroString))
					{
						state.PartialFormatted = ZeroPartialString;
					}
					else
					{
						state.PartialFormatted = ZeroString + state.PartialFormatted;
					}
				}
				else
				{
					state.PartialFormatted = state.PartialFormatted + ZeroString;
				}
			}
			else if (2 < state.PartialFormatted.Length)
			{
				var needCutSecondPartialDigit = state.PartialFormatted.Length == 3
					&& state.CaretPosition == (decimalSeparatorPosition + 1);
				if (needCutSecondPartialDigit)
				{
					state.PartialFormatted = state.PartialFormatted.Remove(1, 1);
				}
				else
				{
					state.PartialFormatted = state.PartialFormatted.Substring(0, 2);
				}
			}
		}

	}
}
