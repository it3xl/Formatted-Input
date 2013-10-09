using System;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Controller;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Helpful method for tests of the <see cref="Process"/> method.
		/// <seealso cref="Process"/>
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void TestProcess(
			String unformattedValue,
			String textBeforeChanging,
			Int32 lastCaretPosition,
			FocusState focusState,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			TextBeforeChanging = textBeforeChanging;
			CaretPositionBeforeTextChanging = lastCaretPosition;
			FocusState = focusState;

			Process(unformattedValue, out resultingFormattedValue, ref caretPosition);
		}

		/// <summary>
		/// The entry pont of the formatting and the caret management.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void Process(
			String unformattedValue,
			out String resultingFormattedValue,
			ref Int32 caretPosition)
		{
			var lastCaretPosition = CaretPositionBeforeTextChanging;

			resultingFormattedValue = unformattedValue;

			try
			{
				var state = new StateController(
						DecimalSeparator,
						PartialDisabledCurrent,
						GroupSeparator,
						TextBeforeChanging
					)
					.GetProcessingStates(
						lastCaretPosition,
						FocusState,
						caretPosition,
						unformattedValue);

				FormatAndManageCaret(state);

				resultingFormattedValue = state.FormattedValue;

				if (state.CaretPosition < 0)
				{
					// It's definetly an error.
					// Fix the impossible negative caret position for sake of the unwanted exception.
					state.CaretPosition = 0;
				}
				if (resultingFormattedValue.Length < state.CaretPosition)
				{
					// It's definetly an error.
					state.CaretPosition = resultingFormattedValue.Length;
				}

				caretPosition = state.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				// Let's set default states, since we screwed up.
				resultingFormattedValue = String.Empty;
				caretPosition = 0;
			}
			finally
			{
				TextBeforeChanging = resultingFormattedValue;
				CaretPositionBeforeTextChanging = caretPosition;
			}
		}

		/// <summary>
		/// The lower implementation of a formatting.
		/// </summary>
		/// <param name="state"></param>
		private void FormatAndManageCaret(ProcessingState state)
		{
			if (SetCaretOnStart(state))
			{
				return;
			}

			DecimalSeparatorAlternatingReplacing(state);

			if(PartialDisabledOnInput
				&& state.PartialFormatted.IsNotNullOrEmpty())
			{
				state.FormattedValue = state.IntegerFormatted + DecimalSeparator + ZerosPartialString;
			}
			if(PartialDisabledOnInput
				&& FocusState == FocusState.JustGotten)
			{
				state.FormattedValue = state.IntegerFormatted;
			}

			DecimalSeparatorDeletedProcessingWithCaret(state);

			// Catch a first digital input and ingnore others.
			if(0 < state.UnformattedValue.Length
				&& state.FormattedValue.Any(el => CustomSerialilzationChars.Contains(el)) == false)
			{
				state.FormattedValue = String.Empty;
				state.CaretPosition = 0;

				// !!!
				return;
			}

			DecimalSeparatorMissed(state);
			DecimalSeparatorExcessiveProcessingWithCaret(state);

			GroupSeparatorProcessingWithCaret(state);

			NotDigitCharsProcessingWithCaret(state);

			state.IntegerFormatting = state.IntegerFormatted;
			state.PartialFormatting = state.PartialFormatted;

			IntegerPartProcessingWithCaret(state);
			PartialPartProcessingWithCaret(state);

			var preliminaryFormattedValue = state.IntegerFormatting;
			if(PartialDisabledCurrent == false)
			{
				preliminaryFormattedValue  += DecimalSeparator + state.PartialFormatting;
			}
			state.FormattedValue = FormatByPrecisionForDouble(preliminaryFormattedValue);

			if(state.JumpCaretToEndOfInteger)
			{
				state.CaretPosition = state.IntegerFormatting.Length;
			}
		}

		/// <summary>
		/// Sets the caret position on the start.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private bool SetCaretOnStart(ProcessingState state)
		{
			if (state.FormattingType != FormattingAfter.CorrectValueResetting)
			{
				return false;
			}
			if (state.CaretPosition != 0)
			{
				return false;
			}

			state.CaretPosition = state.GetIntegerEndCaretPosition();

			return true;
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(ProcessingState state)
		{
			var stringForIteraction = state.FormattedValue;
			foreach (var @char in stringForIteraction)
			{
				if (CustomSerialilzationChars.Contains(@char))
				{
					continue;
				}

				// Let's delete only the first entry of that char for now.
				var index = state.FormattedValue.IndexOf(@char);
				state.FormattedValue = state.FormattedValue.Remove(index, 1);

				if (index <= (state.CaretPosition - 1))
				{
					state.CaretPosition -= 1;
				}
			}
		}

	}
}
