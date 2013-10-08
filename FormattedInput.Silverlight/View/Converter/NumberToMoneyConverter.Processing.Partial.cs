using It3xl.FormattedInput.NullAndEmptyHandling;

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
					if (state.PartialPrevious.StartsWith(ZeroString))
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
