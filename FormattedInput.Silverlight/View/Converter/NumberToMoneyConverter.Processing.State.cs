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
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="caretPosition"></param>
		/// <returns></returns>
		private ProcessingState InitProcessingStates(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			FocusEnum focusState,
			Int32 caretPosition)
		{
			var state = new ProcessingState
				{
					UnformattedValue = unformattedValue,
					FormattedValue = unformattedValue,
					TextBeforeChanging = textBeforeChanging,
					CaretPosition = caretPosition,
				};

			SetFormattingType(state);
			SetJumpCaretToEndOfInteger(focusState, state);
			SetNeedHighlightInput(state);

			SetPreviousStates(state);

			if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
			{
				state.DeletionType = state.CaretPosition < lastCaretPosition
					? DeletionDirection.BackspaceButton
					: DeletionDirection.DeleteButton;

			}

			state.GroupSeparatorDeleted = GetStateGroupSeparatorDeleted(state);

			return state;
		}

		/// <summary>
		/// Defines the formatting type <see cref="FormattingAfter"/>.
		/// </summary>
		/// <param name="state"> </param>
		/// <returns>The value of the  <see cref="FormattingAfter"/></returns>
		private static void SetFormattingType(ProcessingState state)
		{
			FormattingAfter formattingAfter;
			if (String.IsNullOrEmpty(state.TextBeforeChanging))
			{
				formattingAfter = FormattingAfter.EmptyStartValue;
			}
			else if (Math.Abs(state.UnformattedValue.Length - state.TextBeforeChanging.Length) != 1)
			{
				formattingAfter = FormattingAfter.GroupPastingOrDeletion;
			}
			else
			{
				var subtraction = state.UnformattedValue.Length - state.TextBeforeChanging.Length;
				formattingAfter = 0 < subtraction
					? FormattingAfter.OneSymbolAdded
					: FormattingAfter.OneSymbolDeleted;
			}

			state.FormattingType = formattingAfter;
		}

		/// <summary>
		/// Sets the requirements to move the caret to the end of the integer's part position.
		/// </summary>
		/// <param name="focusState"></param>
		/// <param name="state"></param>
		private void SetJumpCaretToEndOfInteger(FocusEnum focusState, ProcessingState state)
		{
			state.JumpCaretToEndOfInteger = false;

			if(focusState != FocusEnum.JustGotten)
			{
				return;
			}
			if(state.FormattingType == FormattingAfter.EmptyStartValue)
			{
				return;
			}
			if (state.UnformattedValue
				.InvokeNotNull(el => el.Length != state.CaretPosition))
			{
				return;
			}

			state.JumpCaretToEndOfInteger = true;
		}

		/// <summary>
		/// Sets state that required fire a custom hilighting of an input.
		/// </summary>
		/// <param name="state"></param>
		private void SetNeedHighlightInput(ProcessingState state)
		{
			// TODO.it3xl.com: SetNeedHighlightInput.
			// JumpCaretToEndOfInteger
			// Input at the end of the partial's part.
		}

		/// <summary>
		/// Sets the previous number states.
		/// </summary>
		/// <param name="state"></param>
		private void SetPreviousStates(ProcessingState state)
		{
			if (state.TextBeforeChanging.IsNullOrEmpty())
			{
				return;
			}

			if (state.TextBeforeChanging.Contains(DecimalSeparator) == false)
			{
				return;
			}

			var number = state.TextBeforeChanging.Split(DecimalSeparator);

			state.PreviousInteger = number.First();
			state.PreviousParatial = number.Last();
		}

		private Boolean? GetStateGroupSeparatorDeleted(ProcessingState state)
		{
			if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
			{
				return null;
			}

			for (var i = 0; i < state.UnformattedValue.Length; i++)
			{
				if (state.UnformattedValue[i] == state.TextBeforeChanging[i])
				{
					continue;
				}

				return state.TextBeforeChanging[i] == GroupSeparator;
			}

			return false;
		}
	}
}
