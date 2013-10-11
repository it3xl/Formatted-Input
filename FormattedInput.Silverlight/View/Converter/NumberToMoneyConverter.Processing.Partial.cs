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
				state.Formatting.Integer.InvokeNotNull(el => el.Length)
				+ DecimalSeparatorChar.Length;

			if (state.Formatting.Partial.Length == 0)
			{
				state.Formatting.Partial = ZerosPartialString;
			}
			else if (state.Formatting.Partial.Length == 1)
			{
				if (state.Formatting.CaretPosition == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.BackspaceButton)
				{
					state.Formatting.Partial = ZeroString + state.Formatting.Partial;
				}
				else if (state.Formatting.CaretPosition == decimalSeparatorPosition
					&& state.OneSymbolDeletionType == DeletionDirection.DeleteButton)
				{
					if (state.PartialPrevious.StartsWith(ZeroString))
					{
						state.Formatting.Partial = ZerosPartialString;
					}
					else
					{
						state.Formatting.Partial = ZeroString + state.Formatting.Partial;
					}
				}
				else
				{
					state.Formatting.Partial = state.Formatting.Partial + ZeroString;
				}
			}
			else if (2 < state.Formatting.Partial.Length)
			{
				var needCutSecondPartialDigit = state.Formatting.Partial.Length == 3
					&& state.Formatting.CaretPosition == (decimalSeparatorPosition + 1);
				if (needCutSecondPartialDigit)
				{
					state.Formatting.Partial = state.Formatting.Partial.Remove(1, 1);
				}
				else
				{
					state.Formatting.Partial = state.Formatting.Partial.Substring(0, 2);
				}
			}
		}

	}
}
