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
		No,

		/// <summary>
		/// Target text-control has the input focus.
		/// </summary>
		Gotten,

		/// <summary>
		/// Focus was just gotten before current text and caret processing.
		/// </summary>
		JustGotten,
	}
}
