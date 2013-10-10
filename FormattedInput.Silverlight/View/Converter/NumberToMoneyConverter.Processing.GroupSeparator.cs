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
					state.CaretPositionForProcessing--;
					state.FormattingValue = state.FormattingValue.Remove(state.CaretPositionForProcessing, 1);
					break;
				case DeletionDirection.DeleteButton:
					state.FormattingValue = state.FormattingValue.Remove(state.CaretPositionForProcessing, 1);
					break;
			}
		}
	}
}
