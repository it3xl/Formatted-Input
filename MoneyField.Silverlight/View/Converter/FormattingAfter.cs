namespace MoneyField.Silverlight.View.Converter
{
	/// <summary>
	/// The type of formattin.
	/// </summary>
	public enum FormattingAfter
	{
		/// <summary>
		/// It's was only the pasting.
		/// </summary>
		GroupPastingOrDeleting,

		/// <summary>
		/// It's the formatting after a call from the <see cref="AnyNumberToMoneyConverter.Convert"/> method.
		/// </summary>
		EmptyStartValue,

		/// <summary>
		/// The input or pasting of a one sybmol.
		/// </summary>
		OneSymbolAdded,

		/// <summary>
		/// Deleting of a one symbol.
		/// </summary>
		OneSymbolDeleted,
	}
}