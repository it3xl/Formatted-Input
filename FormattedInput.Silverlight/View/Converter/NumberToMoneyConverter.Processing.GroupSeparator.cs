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

			switch (state.DeletionType)
			{
				case DeletionDirection.BackspaceButton:
					state.CaretPosition--;
					state.FormattedValue = state.FormattedValue.Remove(state.CaretPosition, 1);
					break;
				case DeletionDirection.DeleteButton:
					state.FormattedValue = state.FormattedValue.Remove(state.CaretPosition, 1);
					break;
			}
		}
	}
}
