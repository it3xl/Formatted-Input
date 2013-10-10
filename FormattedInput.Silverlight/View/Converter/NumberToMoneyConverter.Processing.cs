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
			WriteLogAction(() => " ->  Process");

			var lastCaretPosition = CaretPositionBeforeTextChanging;

			resultingFormattedValue = unformattedValue;

			try
			{

				var state = PrepareStates(unformattedValue, caretPosition, lastCaretPosition);
				FormatAndManageCaret(state);

				resultingFormattedValue = state.FormattingValue;

				caretPosition = state.CaretPositionForProcessing;
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
			JumpCaretFromPartialEndToIntegerEnd(state);

			DecimalSeparatorAlternatingReplacing(state);

			if(PartialDisabledOnInput
				&& state.PartialFormatted.IsNotNullOrEmpty())
			{
				state.FormattingValue = state.IntegerFormatted + DecimalSeparator + ZerosPartialString;
			}
			if(PartialDisabledOnInput
				&& FocusState == FocusState.JustGotten)
			{
				state.FormattingValue = state.IntegerFormatted;
			}

			DecimalSeparatorDeletedProcessingWithCaret(state);

			// Catch a first digital input and ingnore others.
			if(0 < state.UnformattedValue.Length
				&& state.FormattingValue.Any(el => CustomSerialilzationChars.Contains(el)) == false)
			{
				state.FormattingValue = String.Empty;
				state.CaretPositionForProcessing = 0;

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
			state.FormattingValue = FormatByPrecisionForDouble(preliminaryFormattedValue);

			if(state.JumpCaretToEndOfInteger)
			{
				state.CaretPositionForProcessing = state.IntegerFormatting.Length;
			}

			CorrectCaretOnEnd(state);
		}


		/// <summary>
		/// Sets the caret position on the just gotten focus.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		private void JumpCaretFromPartialEndToIntegerEnd(ProcessingState state)
		{
			if(FocusState != FocusState.JustGotten)
			{
				return;
			}
			if(PartialDisabledCurrent)
			{
				return;
			}
			if (state.CaretPositionForProcessing != state.FormattingValue.Length)
			{
				return;
			}

			state.CaretPositionForProcessing = state.GetIntegerEndPosition();
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(ProcessingState state)
		{
			var stringForIteraction = state.FormattingValue;
			foreach (var @char in stringForIteraction)
			{
				if (CustomSerialilzationChars.Contains(@char))
				{
					continue;
				}

				// Let's delete only the first entry of that char for now.
				var index = state.FormattingValue.IndexOf(@char);
				state.FormattingValue = state.FormattingValue.Remove(index, 1);

				if (index <= (state.CaretPositionForProcessing - 1))
				{
					state.CaretPositionForProcessing -= 1;
				}
			}
		}

		private static void CorrectCaretOnEnd(ProcessingState state)
		{
			if (state.CaretPositionForProcessing < 0)
			{
				WriteLogAction(() => "!!! The negative caret position.");

				// It's definetly an error.
				// Fix the impossible negative caret position for sake of the unwanted exception.
				state.CaretPositionForProcessing = 0;
			}
			if (state.FormattingValue.Length < state.CaretPositionForProcessing)
			{
				WriteLogAction(() => "!!! The caret position bigger than the text length.");

				// It's definetly an error.
				state.CaretPositionForProcessing = state.FormattingValue.Length;
			}
		}

	}
}
