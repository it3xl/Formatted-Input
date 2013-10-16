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
			if (1 < state.Formatting.Integer.Length
				&& state.IntegerPrevious == ZeroString
				&& state.Formatting.Integer.Last() == ZeroChar)
			{
				state.Formatting.Integer = state.Formatting.Integer.Remove(state.Formatting.Integer.Length - 1, 1);
			}

			// Leading zeros' processing.
			while (1 < state.Formatting.Integer.Length && state.Formatting.Integer.First() == ZeroChar)
			{
				state.Formatting.Integer = state.Formatting.Integer.Remove(0, 1);
				if (0 < state.Formatting.CaretPosition)
				{
					state.Formatting.CaretPosition--;
				}
			}

			// Before decimal separator should be the 0, if it's the first.
			if (PartialDisabledCurrent == false
				&& state.Formatting.Integer.Length == 0)
			{
				state.Formatting.Integer = ZeroString;

				state.Formatting.CaretPosition++;
			}

			if (GroupSeparator.IsDefault())
			{
				return;
			}

			// The group separator processing.
			var lengthWithoutSeparator = state.Formatting.Integer.Length;
			var caretPositionWithoutGroupSeparator = state.Formatting.CaretPosition;

			var integerInvert = String.Join(null, state.Formatting.Integer.Reverse());
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

				state.Formatting.CaretPosition++;
			}

			state.Formatting.Integer = String.Join(null, integerInvert.Reverse());

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

			state.Formatting.CaretPosition -= separarotAmountAfterCaret;
			if (state.PreservePositionForGroupSeparator)
			{
				state.Formatting.CaretPosition--;
			}
			if (state.OneSymbolDeletionType == DeletionDirection.BackspaceButton
				&& state.GroupSeparatorDeleted == true)
			{
				state.Formatting.CaretPosition++;
			}
		}
	}
}
