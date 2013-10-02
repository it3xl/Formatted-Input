namespace It3xl.FormattedInput.View.Converter
{
	/// <summary>
	/// Describes the critical states of the TextBox's focus.
	/// </summary>
	public enum FocusEnum
	{
		/// <summary>
		/// The default value.
		/// </summary>
		HasNoState,

		/// <summary>
		/// Focus was just gotten before current text and caret processing.
		/// </summary>
		JustGotten,
	}
}
