using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Converter;

namespace It3xl.FormattedInput.View.Controller
{
	/// <summary>
	/// 
	/// </summary>
	internal sealed class StateController
	{
		private readonly Char _decimalSeparator;
		private readonly Char _groupSeparator;
		/// <summary>
		/// A text that was before current processing.
		/// </summary>
		private readonly String _textBeforeChanging;
		/// <summary>
		/// The value before formatting.
		/// </summary>
		private readonly String _unformattedValue;


		internal StateController(char decimalSeparator, char groupSeparator, String textBeforeChanging, String unformattedValue)
		{
			_decimalSeparator = decimalSeparator;
			_groupSeparator = groupSeparator;
			_textBeforeChanging = textBeforeChanging;
			_unformattedValue = unformattedValue;
		}

		/// <summary>
		/// Initializes states for formatting.
		/// </summary>
		/// <param name="lastCaretPosition"></param>
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="caretPosition"></param>
		/// <returns></returns>
		internal ProcessingState GetProcessingStates(
			Int32 lastCaretPosition,
			FocusEnum focusState,
			Int32 caretPosition)
		{
			var state = new ProcessingState
				{
					FormattedValue = _unformattedValue,
					CaretPosition = caretPosition,
				};

			SetFormattingType(state);
			SetJumpCaretToEndOfInteger(focusState, state);
			SetNeedHighlightInput(state);

			SetPreviousStates(state);

			if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
			{
				state.OneSymbolDeletionType = state.CaretPosition < lastCaretPosition
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
		private void SetFormattingType(ProcessingState state)
		{
			FormattingAfter formattingAfter;
			if (String.IsNullOrEmpty(_textBeforeChanging))
			{
				formattingAfter = FormattingAfter.EmptyStartValue;
			}
			else if (Math.Abs(_unformattedValue.Length - _textBeforeChanging.Length) != 1)
			{
				formattingAfter = FormattingAfter.GroupPastingOrDeletion;
			}
			else
			{
				var subtraction = _unformattedValue.Length - _textBeforeChanging.Length;
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

			if (focusState != FocusEnum.JustGotten)
			{
				return;
			}
			if (state.FormattingType == FormattingAfter.EmptyStartValue)
			{
				return;
			}
			if (_unformattedValue
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
			// Invalid Input.
			// Invalid input to the empty field.
			// Integer input exceeding the bitness of the integer part.
		}

		/// <summary>
		/// Sets the previous number states.
		/// </summary>
		/// <param name="state"></param>
		private void SetPreviousStates(ProcessingState state)
		{
			if (_textBeforeChanging.IsNullOrEmpty())
			{
				return;
			}

			if (_textBeforeChanging.Contains(_decimalSeparator) == false)
			{
				return;
			}

			var number = _textBeforeChanging.Split(_decimalSeparator);

			state.IntegerPrevious = number.First();
			state.PartialPrevious = number.Last();
		}

		private Boolean? GetStateGroupSeparatorDeleted(ProcessingState state)
		{
			if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
			{
				return null;
			}

			for (var i = 0; i < _unformattedValue.Length; i++)
			{
				if (_unformattedValue[i] == _textBeforeChanging[i])
				{
					continue;
				}

				return _textBeforeChanging[i] == _groupSeparator;
			}

			return false;
		}
	}
}
