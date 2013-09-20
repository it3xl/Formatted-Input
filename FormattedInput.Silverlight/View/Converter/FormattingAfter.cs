namespace It3xl.FormattedInput.View.Converter
{
	/// <summary>
	/// The type of formattin.
	/// </summary>
	internal enum FormattingAfter
	{
		/// <summary>
		/// It's was only the pasting.
		/// </summary>
		GroupPastingOrDeletion,

		/// <summary>
		/// It's the formatting after a call from the <see cref="NumberToMoneyConverter.Convert"/> method.
		/// </summary>
		EmptyStartValue,

		/// <summary>
		/// The input or pasting of a one sybmol.
		/// </summary>
		OneSymbolAdded,

		/// <summary>
		/// Deletion of a one symbol.
		/// </summary>
		OneSymbolDeleted,
	}
}