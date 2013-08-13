namespace MoneyField.Silverlight.View.Converter
{
	/// <summary>
	/// The type of deleting.
	/// </summary>
	public enum DeleteDirection
	{
		/// <summary>
		/// It was the deleting by the Backspace button. The caret position was decreased on
		/// </summary>
		BackspaceButton,

		/// <summary>
		/// It was the deleting by the Delete Button or by the cuttin of a one symbol.
		/// </summary>
		DeleteButton,
	}
}