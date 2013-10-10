using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Controller;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Processing of the partial part of a number.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private void PartialPartProcessingWithCaret(ProcessingState state)
		{
			if (PartialDisabledCurrent)
			{
				return;
			}

			// After the decimal separator should be two digits for now (PartialBitness is not implemented).

			var decimalSeparatorPosition =
				state.IntegerFormatting.InvokeNotNull(el => el.Length)
				+ DecimalSeparatorChar.Length;

			if (state.PartialFormatting.Length == 0)
			{
				state.PartialFormatting = ZerosPartialString;
			}
			else if (state.PartialFormatting.Length == 1)
			{
				if (state.CaretPositionForProcessing == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.BackspaceButton)
				{
					state.PartialFormatting = ZeroString + state.PartialFormatting;
				}
				else if (state.CaretPositionForProcessing == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.DeleteButton)
				{
					if (state.PartialPrevious.StartsWith(ZeroString))
					{
						state.PartialFormatting = ZerosPartialString;
					}
					else
					{
						state.PartialFormatting = ZeroString + state.PartialFormatting;
					}
				}
				else
				{
					state.PartialFormatting = state.PartialFormatting + ZeroString;
				}
			}
			else if (2 < state.PartialFormatting.Length)
			{
				var needCutSecondPartialDigit = state.PartialFormatting.Length == 3
					&& state.CaretPositionForProcessing == (decimalSeparatorPosition + 1);
				if (needCutSecondPartialDigit)
				{
					state.PartialFormatting = state.PartialFormatting.Remove(1, 1);
				}
				else
				{
					state.PartialFormatting = state.PartialFormatting.Substring(0, 2);
				}
			}
		}

	}
}
