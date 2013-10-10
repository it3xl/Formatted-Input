using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Converter;

namespace It3xl.FormattedInput.View.Controller
{
	internal sealed partial class ProcessingState
	{

		/// <summary>
		/// An encapsulation for states' processing of the <see cref="ProcessingState"/>.
		/// </summary>
		internal sealed class StateController
		{
			internal readonly Char DecimalSeparator;
			internal readonly Boolean PartialDisabled;

			private readonly Char _groupSeparator;
			/// <summary>
			/// A text that was before current processing.
			/// </summary>
			private readonly String _textBeforeChanging;

			internal StateController(
				Char decimalSeparator,
				Boolean partialDisabled,
				Char groupSeparator,
				String textBeforeChanging
				)
			{
				DecimalSeparator = decimalSeparator;
				PartialDisabled = partialDisabled;

				_groupSeparator = groupSeparator;
				_textBeforeChanging = textBeforeChanging;
			}

			/// <summary>
			/// Initializes states for formatting.
			/// </summary>
			/// <param name="lastCaretPosition"></param>
			/// <param name="focusState">The critical state of the TextBox's focus.</param>
			/// <param name="caretPosition"></param>
			/// <param name="unformattedValue"></param>
			/// <param name="jumpCaretToEndOfInteger"></param>
			/// <returns></returns>
			internal ProcessingState GetProcessingStates(
				Int32 lastCaretPosition,
				FocusState focusState,
				Int32 caretPosition,
				String unformattedValue,
				Boolean jumpCaretToEndOfInteger
				)
			{
				var state = new ProcessingState(unformattedValue, jumpCaretToEndOfInteger)
					{
						Controller = this,
						CaretPositionForProcessing = caretPosition,
					};

				SetFormattingType(state);
				SetNeedHighlightInput(state);

				SetPreviousStates(state);

				if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
				{
					state.OneSymbolDeletionType = state.CaretPositionForProcessing < lastCaretPosition
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
				else if (state.UnformattedValue == _textBeforeChanging)
				{
					formattingAfter = FormattingAfter.CorrectValueResetting;
				}
				else if (Math.Abs(state.UnformattedValue.Length - _textBeforeChanging.Length) != 1)
				{
					formattingAfter = FormattingAfter.GroupPastingOrDeletion;
				}
				else
				{
					var subtraction = state.UnformattedValue.Length - _textBeforeChanging.Length;
					formattingAfter = 0 < subtraction
						? FormattingAfter.OneSymbolAdded
						: FormattingAfter.OneSymbolDeleted;
				}

				state.FormattingType = formattingAfter;
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

				var number = _textBeforeChanging.Split(DecimalSeparator);

				state.IntegerPrevious = number.First();

				state.PartialPrevious = PartialDisabled ? String.Empty : number.Last();
			}

			private Boolean? GetStateGroupSeparatorDeleted(ProcessingState state)
			{
				if (state.FormattingType != FormattingAfter.OneSymbolDeleted)
				{
					return null;
				}

				for (var i = 0; i < state.UnformattedValue.Length; i++)
				{
					if (state.UnformattedValue[i] == _textBeforeChanging[i])
					{
						continue;
					}

					return _textBeforeChanging[i] == _groupSeparator;
				}

				return false;
			}
		}
	}
}
