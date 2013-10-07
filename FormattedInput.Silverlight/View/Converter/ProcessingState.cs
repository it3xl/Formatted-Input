// ReSharper disable ConvertToAutoProperty

using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

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
		/// The value before formatting.
		/// </summary>
		public readonly String UnformattedValue;

		public ProcessingState(String unformattedValue, Char decimalSeparator, Boolean partialDisabled)
		{
			UnformattedValue = unformattedValue;
			FormattedValue = unformattedValue;

			_decimalSeparator = decimalSeparator;

			_partialDisabled = partialDisabled;
		}

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
		internal DeletionDirection? OneSymbolDeletionType { get; set; }

		/// <summary>
		/// The sign that the Group Separator has been deleted.
		/// </summary>
		internal Boolean? GroupSeparatorDeleted { get; set; }

		/// <summary>
		/// The previous integer part.
		/// </summary>
		internal string IntegerPrevious { get; set; }

		/// <summary>
		/// The formatted integer part.
		/// </summary>
		internal string IntegerFormatted { get; set; }

		/// <summary>
		/// The previous partial part.
		/// </summary>
		internal string PartialPrevious { get; set; }

		/// <summary>
		/// The formatted partial part.
		/// </summary>
		internal string PartialFormatted { get; set; }

		/// <summary>
		/// Requires to move the caret to the end of the integer's part position.
		/// </summary>
		public Boolean JumpCaretToEndOfInteger { get; set; }

		/// <summary>
		/// The decimal separator.
		/// </summary>
		private readonly Char _decimalSeparator;

		/// <summary>
		/// .
		/// </summary>
		private readonly Boolean _partialDisabled;


		/// <summary>
		/// Returns current position for the end of integer.
		/// </summary>
		/// <returns></returns>
		internal Int32 GetIntegerEndCaretPosition()
		{
			var result = _partialDisabled
				? FormattedValue.InvokeNotNull(el => el.Length)
				: FormattedValue.IndexOf(_decimalSeparator);

			return result;
		}

		/// <summary>
		/// Gets the current integer part of a number.
		/// </summary>
		/// <returns></returns>
		internal String GetInteger()
		{
			if(_partialDisabled)
			{
				return FormattedValue;
			}

			return FormattedValue.Split(_decimalSeparator).First();
		}

		/// <summary>
		/// Gets the current patial part of a number.
		/// </summary>
		/// <returns></returns>
		internal String GetPartial()
		{
			if(_partialDisabled)
			{
				return String.Empty;
			}

			return FormattedValue.Split(_decimalSeparator).Last();
		}

	}
}