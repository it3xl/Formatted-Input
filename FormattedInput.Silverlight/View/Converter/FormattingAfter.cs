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
		/// Control has empty value now and earlier.
		/// </summary>
		EmptyValueBeforeAndNow,

		/// <summary>
		/// Control was not empty but now become.
		/// </summary>
		EmptyValueBecome,

		/// <summary>
		/// It's the formatting of focus. Also it rises just after <see cref="EmptyValueBeforeAndNow"/>,
		///  in the <see cref="NumberToMoneyConverter.ConvertBack"/> method.
		/// </summary>
		CorrectValueResetting,

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