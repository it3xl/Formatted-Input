// ReSharper disable ConvertToAutoProperty
using System;

namespace MoneyField.Silverlight.View.Converter
{
	internal sealed class FormatterState
	{
		private string _formattedValue;
		private int _caretPosition;

		/// <summary>
		/// The money representation.
		/// </summary>
		public String FormattedValue
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
		public Int32 CaretPosition
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
		public FormattingAfter FormattingType { get; set; }

		/// <summary>
		/// Type of the deletion for the <see cref="FormattingType"/>
		///  == <see cref="FormattingAfter"/>.<see cref="FormattingAfter.OneSymbolDeleted"/>.
		/// </summary>
		public DeletionDirection? DeletionType { get; set; }

		/// <summary>
		/// The sign that the Group Separator has been deleted.
		/// </summary>
		public Boolean? GroupSeparatorDeleted { get; set; }

		/// <summary>
		/// The value before formatting.
		/// </summary>
		public String UnformattedValue { get; set; }


		/// <summary>
		/// The previous integer part.
		/// </summary>
		public string PreviousInteger { get; set; }

		/// <summary>
		/// The previous partial part.
		/// </summary>
		public string PreviousParatial { get; set; }
	}
}