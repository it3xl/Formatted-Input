// ReSharper disable ConvertToAutoProperty
using System;

namespace MoneyField.Silverlight.View.Converter
{
	internal sealed class FormatterState
	{
		private string _formatteValue;
		private int _caretPosition;

		/// <summary>
		/// The money representation.
		/// </summary>
		public String FormatteValue
		{
			get
			{
				return _formatteValue;
			}
			set
			{
				_formatteValue = value;
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
		/// The value before formatting.
		/// </summary>
		public string UnformattedValue { get; set; }
	}
}