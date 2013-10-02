// ReSharper disable ConvertToAutoProperty

using System;

namespace It3xl.FormattedInput.View.Converter
{
	/// <summary>
	/// The state for a processing of a text and a caret position.
	/// </summary>
	internal sealed class ProcessingState
	{
		private string _formattedValue;
		private int _caretPosition;

		/// <summary>
		/// The money representation.
		/// </summary>
		internal String FormattedValue
		{
			get
			{
				return _formattedValue;
			}
			set
			{
				_formattedValue = value;
			}
		}

		/// <summary>
		/// A text that was before current processing.
		/// </summary>
		public string TextBeforeChanging { get; set; }

		/// <summary>
		/// The caret (cursor) position.
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
		/// Type of formatting.
		/// </summary>
		internal FormattingAfter FormattingType { get; set; }

		/// <summary>
		/// Type of the deletion for the <see cref="FormattingType"/>
		///  == <see cref="FormattingAfter"/>.<see cref="FormattingAfter.OneSymbolDeleted"/>.
		/// </summary>
		internal DeletionDirection? DeletionType { get; set; }

		/// <summary>
		/// The sign that the Group Separator has been deleted.
		/// </summary>
		internal Boolean? GroupSeparatorDeleted { get; set; }

		/// <summary>
		/// The value before formatting.
		/// </summary>
		internal String UnformattedValue { get; set; }


		/// <summary>
		/// The previous integer part.
		/// </summary>
		internal string PreviousInteger { get; set; }

		/// <summary>
		/// The previous partial part.
		/// </summary>
		internal string PreviousParatial { get; set; }

		/// <summary>
		/// Requires to move the caret to the end of the integer's part position.
		/// </summary>
		public Boolean JumpCaretToEndOfInteger { get; set; }

	}
}