namespace MoneyField.Silverlight.View.Converter
{
	/// <summary>
	/// The type of deletion for the
	///  <see cref="FormattingAfter"/>.<see cref="FormattingAfter.OneSymbolDeleted"/>.
	/// </summary>
	public enum DeletionDirection
	{
		/// <summary>
		/// It was the deletion by the Backspace button. The caret position was decreased on
		/// </summary>
		BackspaceButton,

		/// <summary>
		/// It was the deletion by the Delete Button or by the cuttin of a one symbol.
		/// </summary>
		DeleteButton,
	}
}