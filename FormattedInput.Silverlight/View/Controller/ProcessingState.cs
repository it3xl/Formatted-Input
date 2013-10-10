// ReSharper disable ConvertToAutoProperty

using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Converter;

namespace It3xl.FormattedInput.View.Controller
{
	/// <summary>
	/// The state for an one processing of a text and a caret position.
	/// </summary>
	internal sealed partial class ProcessingState
	{
		/// <summary>
		/// The value before formatting.
		/// </summary>
		public readonly String UnformattedValue;

		private ProcessingState(String unformattedValue, Boolean jumpCaretToEndOfInteger)
		{
			FormattingValue = unformattedValue;

			UnformattedValue = unformattedValue;
			JumpCaretToEndOfInteger = jumpCaretToEndOfInteger;
		}

		/// <summary>
		/// The money representation.
		/// </summary>
		internal String FormattingValue { get; set; }

		private int _caretPositionForProcessing;
		/// <summary>
		/// The position of the caret (cursor).
		/// </summary>
		internal Int32 CaretPositionForProcessing
		{
			get
			{
				return _caretPositionForProcessing;
			}
			set
			{
				_caretPositionForProcessing = value;
			}
		}

		/// <summary>
		/// The formatted integer part.
		/// </summary>
		internal string IntegerFormatting { get; set; }

		/// <summary>
		/// The formatted partial part.
		/// </summary>
		internal string PartialFormatting { get; set; }


		/// <summary>
		/// Type of formatting.
		/// </summary>
		internal FormattingAfter FormattingType { get; private set; }

		/// <summary>
		/// Type of the deletion for the <see cref="FormattingType"/>
		///  == <see cref="FormattingAfter"/>.<see cref="FormattingAfter.OneSymbolDeleted"/>.
		/// </summary>
		internal DeletionDirection? OneSymbolDeletionType { get; private set; }

		/// <summary>
		/// The sign that the Group Separator has been deleted.
		/// </summary>
		internal Boolean? GroupSeparatorDeleted { get; private set; }

		/// <summary>
		/// The previous integer part.
		/// </summary>
		internal string IntegerPrevious { get; private set; }

		/// <summary>
		/// The previous partial part.
		/// </summary>
		internal string PartialPrevious { get; private set; }

		/// <summary>
		/// Requires to move the caret to the end of the integer's part position.
		/// </summary>
		public Boolean JumpCaretToEndOfInteger { get; private set; }


		/// <summary>
		/// Holder of raw states.
		/// </summary>
		internal StateController Controller { get; private set; }


		/// <summary>
		/// Current formatted integer part of a number.
		/// </summary>
		/// <returns></returns>
		internal String IntegerFormatted
		{
			get
			{
				return FormattingValue.Split(DecimalSeparator).First();
			}
		}

		/// <summary>
		/// Current formatted patial part of a number.
		/// </summary>
		/// <returns></returns>
		internal String PartialFormatted
		{
			get
			{
				if (PartialDisabled)
				{
					return String.Empty;
				}

				return FormattingValue.Split(DecimalSeparator).Last();
			}
		}


		/// <summary>
		/// The decimal separator.
		/// </summary>
		private Char DecimalSeparator
		{
			get
			{
				return Controller.DecimalSeparator;
			}
		}

		/// <summary>
		/// The partial part is disabled.
		/// </summary>
		private Boolean PartialDisabled
		{
			get
			{
				return Controller.PartialDisabled;
			}
		}


		/// <summary>
		/// Returns current position for the end of integer for the caret (cursor).
		/// </summary>
		/// <returns></returns>
		internal Int32 GetIntegerEndPosition()
		{
			var result = PartialDisabled
				? FormattingValue.InvokeNotNull(el => el.Length)
				: FormattingValue.IndexOf(DecimalSeparator);

			return result;
		}

	}
}