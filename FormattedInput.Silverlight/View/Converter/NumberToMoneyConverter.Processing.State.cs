using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Initializes states for formatting.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging"></param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		/// <returns></returns>
		private FormatterState InitProcessingStates(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			String resultingFormattedValue,
			Int32 caretPosition)
		{
			var state = new FormatterState
			{
				FormattedValue = resultingFormattedValue,
				CaretPosition = caretPosition,
				FormattingType = SetFormattingType(unformattedValue, textBeforeChanging),
			};

			SetPreviousStates(state, textBeforeChanging);

			if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
			{
				state.DeletionType = state.CaretPosition < lastCaretPosition
					? DeletionDirection.BackspaceButton
					: DeletionDirection.DeleteButton;

			}

			state.GroupSeparatorDeleted = GetStateGroupSeparatorDeleted(state.FormattingType, unformattedValue, textBeforeChanging);

			return state;
		}

		/// <summary>
		/// Defines the formatting type <see cref="FormattingAfter"/>.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging"></param>
		/// <returns>The value of the  <see cref="FormattingAfter"/></returns>
		private static FormattingAfter SetFormattingType(String unformattedValue, String textBeforeChanging)
		{
			FormattingAfter formattingAfter;
			if (String.IsNullOrEmpty(textBeforeChanging))
			{
				formattingAfter = FormattingAfter.EmptyStartValue;
			}
			else if (Math.Abs(unformattedValue.Length - textBeforeChanging.Length) != 1)
			{
				formattingAfter = FormattingAfter.GroupPastingOrDeletion;
			}
			else
			{
				var subtraction = unformattedValue.Length - textBeforeChanging.Length;
				formattingAfter = 0 < subtraction
					? FormattingAfter.OneSymbolAdded
					: FormattingAfter.OneSymbolDeleted;
			}

			return formattingAfter;
		}

		/// <summary>
		/// Sets the previous number states.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="textBeforeChanging"></param>
		private void SetPreviousStates(FormatterState state, string textBeforeChanging)
		{
			if (textBeforeChanging.IsNullOrEmpty())
			{
				return;
			}

			if (textBeforeChanging.Contains(DecimalSeparator) == false)
			{
				return;
			}

			var number = textBeforeChanging.Split(DecimalSeparator);

			state.PreviousInteger = number.First();
			state.PreviousParatial = number.Last();
		}

		private Boolean? GetStateGroupSeparatorDeleted(FormattingAfter formattingType, String unformattedValue, String textBeforeChanging)
		{
			if (formattingType != FormattingAfter.OneSymbolDeleted)
			{
				return null;
			}

			for (var i = 0; i < unformattedValue.Length; i++)
			{
				if (unformattedValue[i] == textBeforeChanging[i])
				{
					continue;
				}

				return textBeforeChanging[i] == GroupSeparator;
			}

			return false;
		}
	}
}
