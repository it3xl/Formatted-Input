// ReSharper disable ConvertToAutoProperty

using System;

namespace It3xl.FormattedInput.View.Converter
{
	internal sealed class FormatterState
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
	}
}