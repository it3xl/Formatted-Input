using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Controller;

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
			if (1 < state.IntegerFormatting.Length
				&& state.IntegerPrevious == ZeroString
				&& state.IntegerFormatting.Last() == ZeroChar)
			{
				state.IntegerFormatting = state.IntegerFormatting.Remove(state.IntegerFormatting.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < state.IntegerFormatting.Length && state.IntegerFormatting.First() == ZeroChar)
			{
				state.IntegerFormatting = state.IntegerFormatting.Remove(0, 1);
				if (0 < state.CaretPositionForProcessing)
				{
					state.CaretPositionForProcessing--;
				}
			}

			// Before decimal separator should be the 0, if it's the first.
			if (PartialDisabledCurrent == false
				&& state.IntegerFormatting.Length == 0)
			{
				state.IntegerFormatting = ZeroString;

				state.CaretPositionForProcessing++;
			}

			if (GroupSeparator.IsDefault())
			{
				return;
			}

			// The group separator processing.
			var lengthWithoutSeparator = state.IntegerFormatting.Length;
			var caretPositionWithoutGroupSeparator = state.CaretPositionForProcessing;

			var integerInvert = String.Join(null, state.IntegerFormatting.Reverse());
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

				state.CaretPositionForProcessing++;
			}

			state.IntegerFormatting = String.Join(null, integerInvert.Reverse());

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

			state.CaretPositionForProcessing -= separarotAmountAfterCaret;
		}
	}
}
