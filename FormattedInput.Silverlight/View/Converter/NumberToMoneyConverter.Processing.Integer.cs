using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
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
			if (PartialDisabledCurrent == false
				&& state.IntegerFormatted.Length == 0)
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
	}
}
