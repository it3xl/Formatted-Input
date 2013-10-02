using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Excessive Decimal Separator's processing.
		/// With caret managment.
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorExcessiveProcessingWithCaret(ProcessingState state)
		{
			var separatorCount = state.FormattedValue.Count(el => el == DecimalSeparator);
			if (separatorCount < 2)
			{
				return;
			}

			// Let's stay just the first decimal separator.
			var index = state.FormattedValue.IndexOf(DecimalSeparator);
			state.FormattedValue = state.FormattedValue
				.Replace(DecimalSeparatorChar, String.Empty)
				.Insert(index, DecimalSeparatorChar);

			if (index + 1 < state.CaretPosition)
			{
				state.CaretPosition -= separatorCount - 1;
			}
		}

		/// <summary>
		/// Yet another missed Decimal Separator processing.
		/// <seealso cref="DecimalSeparatorDeletedProcessingWithCaret"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorMissed(ProcessingState state)
		{
			var separatorCountV = state.FormattedValue.Count(el => el == DecimalSeparator);
			if (0 < separatorCountV)
			{
				return;
			}

			state.FormattedValue += DecimalSeparator;
		}

		/// <summary>
		/// Restores the Decimal Separator if it lost and manage caret position.
		/// <seealso cref="DecimalSeparatorMissed"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorDeletedProcessingWithCaret(ProcessingState state)
		{
			if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
			{
				return;
			}

			// Restoring of the Decimal Separator and carret promotion.
			// The Decimal Separator delition means onle caret promotion.
			var wasDeletedSeparator = state.FormattedValue.Contains(DecimalSeparator) == false;
			if (wasDeletedSeparator == false)
			{
				return;
			}

			state.FormattedValue = state.FormattedValue.Insert(state.CaretPosition, DecimalSeparatorChar);

			if (state.DeletionType == DeletionDirection.DeleteButton)
			{
				state.CaretPosition++;
			}
		}

		/// <summary>
		/// Replaces an alternate Decimal Separator.
		/// <see cref="DecimalSeparatorAlternative"/>
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorAlternatingReplacing(ProcessingState state)
		{
			if (DecimalSeparatorAlternative.IsDefault())
			{
				return;
			}

			if (DecimalSeparatorAlternative == DecimalSeparator)
			{
				return;
			}

			state.FormattedValue = state.FormattedValue
				.Replace(DecimalSeparatorAlternative, DecimalSeparator);
		}
	}
}
