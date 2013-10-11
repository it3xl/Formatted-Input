using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Controller;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Processing for the excessive Decimal Separators.<para/>
		/// With caret managment.
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorExcessiveProcessingWithCaret(ProcessingState state)
		{
			if(PartialDisabledCurrent)
			{
				return;
			}

			var separatorCount = state.Formatting.Text.Count(el => el == DecimalSeparator);
			if (separatorCount < 2)
			{
				return;
			}

			// Let's stay just the first decimal separator.
			var index = state.Formatting.Text.IndexOf(DecimalSeparator);
			state.Formatting.Text = state.Formatting.Text
				.Replace(DecimalSeparatorChar, String.Empty)
				.Insert(index, DecimalSeparatorChar);

			if (index + 1 < state.Formatting.CaretPosition)
			{
				state.Formatting.CaretPosition -= separatorCount - 1;
			}
		}

		/// <summary>
		/// Yet another missed Decimal Separator processing.
		/// <seealso cref="DecimalSeparatorDeletedProcessingWithCaret"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorMissed(ProcessingState state)
		{
			if(PartialDisabledCurrent)
			{
				return;
			}

			var separatorCount = state.Formatting.Text.Count(el => el == DecimalSeparator);
			if (0 < separatorCount)
			{
				return;
			}

			state.Formatting.Text += DecimalSeparator;
		}

		/// <summary>
		/// Restores the Decimal Separator if it lost and manage caret position.
		/// <seealso cref="DecimalSeparatorMissed"/> 
		/// </summary>
		/// <param name="state"></param>
		private void DecimalSeparatorDeletedProcessingWithCaret(ProcessingState state)
		{
			if(PartialDisabledCurrent)
			{
				return;
			}

			if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
			{
				return;
			}

			// Restoring of the Decimal Separator and carret promotion.
			// The Decimal Separator delition means onle caret promotion.
			var wasDeletedSeparator = state.Formatting.Text.Contains(DecimalSeparator) == false;
			if (wasDeletedSeparator == false)
			{
				return;
			}

			state.Formatting.Text = state.Formatting.Text.Insert(state.Formatting.CaretPosition, DecimalSeparatorChar);

			if (state.OneSymbolDeletionType == DeletionDirection.DeleteButton)
			{
				state.Formatting.CaretPosition++;
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

			state.Formatting.Text = state.Formatting.Text
				.Replace(DecimalSeparatorAlternative, DecimalSeparator);
		}
	}
}
