using It3xl.FormattedInput.View.Controller;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Manage and format for the Group Separator.
		/// </summary>
		/// <param name="state"></param>
		private void GroupSeparatorProcessingWithCaret(ProcessingState state)
		{
			if (state.GroupSeparatorDeleted != true)
			{
				return;
			}

			switch (state.OneSymbolDeletionType)
			{
				case DeletionDirection.BackspaceButton:
					state.Formatting.CaretPosition--;
					state.Formatting.Text = state.Formatting.Text.Remove(state.Formatting.CaretPosition, 1);
					break;
				case DeletionDirection.DeleteButton:
					state.Formatting.Text = state.Formatting.Text.Remove(state.Formatting.CaretPosition, 1);
					break;
			}
		}
	}
}
