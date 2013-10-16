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
			private readonly String _textBeforeChangingNotNull;

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
				_textBeforeChangingNotNull = textBeforeChanging ?? String.Empty;
			}

			/// <summary>
			/// Initializes states for formatting.
			/// </summary>
			/// <param name="lastCaretPosition">The last caret position before the formatting.</param>
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
						Formatting = { CaretPosition = caretPosition },
					};

				SetFormattingType(state);
				SetNeedHighlightInput(state);

				SetPreviousStates(state);

				if (state.FormattingType == FormattingAfter.OneSymbolDeleted)
				{
					state.OneSymbolDeletionType = state.Formatting.CaretPosition < lastCaretPosition
						? DeletionDirection.BackspaceButton
						: DeletionDirection.DeleteButton;

				}

				state.GroupSeparatorDeleted = GetStateGroupSeparatorDeleted(state);

				SetPreservePositionForGroupSeparator(state);
				SetPreservePositionForDeletionOfDigitBeforeGroupSeparator(state);

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
				if (_textBeforeChangingNotNull.IsNullOrEmpty()
					&& state.UnformattedValue.IsNullOrEmpty())
				{
					formattingAfter = FormattingAfter.EmptyValueBeforeAndNow;
				}
				else if (_textBeforeChangingNotNull.IsNotNullOrEmpty()
					&& state.UnformattedValue.IsNullOrEmpty())
				{
					formattingAfter = FormattingAfter.EmptyValueBecome;
				}
				else if (state.UnformattedValue == _textBeforeChangingNotNull)
				{
					formattingAfter = FormattingAfter.ResettingTheSame;
				}
				else if (Math.Abs(state.UnformattedValue.Length - _textBeforeChangingNotNull.Length) != 1)
				{
					formattingAfter = FormattingAfter.GroupPastingOrDeletion;
				}
				else
				{
					var subtraction = state.UnformattedValue.Length - _textBeforeChangingNotNull.Length;
					formattingAfter = 0 < subtraction
						? FormattingAfter.OneSymbolAdded
						: FormattingAfter.OneSymbolDeleted;
				}

				state.FormattingType = formattingAfter;

				NumberToMoneyConverter.WriteLogAction(() => String.Format("  !! FormattingType = {0}", formattingAfter));
			}

			/// <summary>
			/// Sets state that required fire a custom hilighting of an input.
			/// </summary>
			/// <param name="state"></param>
			private void SetNeedHighlightInput(ProcessingState state)
			{
				// it3xl.com: It's a feature for the future.
			}

			/// <summary>
			/// Sets the previous number states.
			/// </summary>
			/// <param name="state"></param>
			private void SetPreviousStates(ProcessingState state)
			{
				if (_textBeforeChangingNotNull.IsNullOrEmpty())
				{
					return;
				}

				var number = _textBeforeChangingNotNull.Split(DecimalSeparator);

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
					if (state.UnformattedValue[i] == _textBeforeChangingNotNull[i])
					{
						continue;
					}

					return _textBeforeChangingNotNull[i] == _groupSeparator;
				}

				return false;
			}

			/// <summary>
			/// <see cref="ProcessingState.PreservePositionForGroupSeparatorOnFocus"/>.
			/// </summary>
			/// <param name="state"></param>
			private void SetPreservePositionForGroupSeparator(ProcessingState state)
			{
				if (state.FormattingType != FormattingAfter.ResettingTheSame)
				{
					return;
				}
				if (_groupSeparator == Char.MinValue)
				{
					// The Group Separator don't set.
					return;
				}

				var charAfterCaret = state.UnformattedValue.ElementAtOrDefault(state.Formatting.CaretPosition);
				var notGroupSeparator = charAfterCaret != _groupSeparator;
				if (notGroupSeparator)
				{
					return;
				}

				state.PreservePositionForGroupSeparatorOnFocus = true;
			}

			/// <summary>
			/// <see cref="ProcessingState.PreservePositionForDeletionOfDigitBeforeGroupSeparator"/>.
			/// </summary>
			/// <param name="state"></param>
			private void SetPreservePositionForDeletionOfDigitBeforeGroupSeparator(ProcessingState state)
			{
				if (state.OneSymbolDeletionType != DeletionDirection.DeleteButton)
				{
					return;
				}
				if (_groupSeparator == Char.MinValue)
				{
					// The Group Separator don't set.
					return;
				}

				var charAfterCaret = state.UnformattedValue.ElementAtOrDefault(state.Formatting.CaretPosition);
				var notGroupSeparator = charAfterCaret != _groupSeparator;
				if (notGroupSeparator)
				{
					return;
				}

				state.PreservePositionForDeletionOfDigitBeforeGroupSeparator = true;
			}

		}
	}
}
