using System;

namespace It3xl.FormattedInput.View.Controller
{
	public class FormattingState
	{
		/// <summary>
		/// The money representation.
		/// </summary>
		internal String Text { get; set; }

		private int _caretPosition;
		/// <summary>
		/// The position of the caret (cursor).
		/// </summary>
		internal Int32 CaretPosition
		{
			get
			{
				return _caretPosition;
			}
			set
			{
				_caretPosition = value;
			}
		}

		/// <summary>
		/// The formatted integer part.
		/// </summary>
		internal string Integer { get; set; }

		/// <summary>
		/// The formatted partial part.
		/// </summary>
		internal string Partial { get; set; }
	}
}