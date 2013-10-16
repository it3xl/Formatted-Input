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
		/// <param name="focusState">The critical state of the TextBox's focus.</param>
		/// <param name="runtimeType"> </param>
		/// <param name="unformattedValue"></param>
		/// <param name="textBeforeChanging">
		/// The previous text value.
		/// Must have the null value if it is the call from the <see cref="Convert"/> method.
		/// </param>
		/// <param name="lastCaretPosition"></param>
		/// <param name="resultingFormattedValue"></param>
		/// <param name="caretPosition"></param>
		public void TestProcess(FocusState focusState, RuntimeType runtimeType, string unformattedValue, string textBeforeChanging, int lastCaretPosition, out string resultingFormattedValue, ref int caretPosition)
		{
			TextBeforeChangingNotNull = textBeforeChanging;
			CaretPositionBeforeTextChanging = lastCaretPosition;
			FocusState = focusState;
			RuntimeType = runtimeType;

			Process(unformattedValue, out resultingFormattedValue, ref caretPosition);

			// !!! Be aware of this in tests!
			FocusState = FocusState.No;
			RuntimeType = RuntimeType.NotInitialized;
		}

		/// <summary>
		/// The entry pont of the formatting and the caret management.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="text"></param>
		/// <param name="caretPosition"></param>
		public void Process(
			String unformattedValue,
			out String text,
			ref Int32 caretPosition)
		{
			WriteLogAction(() => " ->  Process");

			var lastCaretPosition = CaretPositionBeforeTextChanging;

			text = unformattedValue??String.Empty;

			try
			{
				var state = PrepareStates(unformattedValue, caretPosition, lastCaretPosition);
				FormatAndManageCaret(state);

				text = state.Formatting.Text;

				caretPosition = state.Formatting.CaretPosition;
			}
			catch (Exception ex)
			{
				ShowExeptionAction.InvokeNotNull(action => action(ex));

				// Let's set default states, since we screwed up.
				text = String.Empty;
				caretPosition = 0;
			}
			finally
			{
				TextBeforeChangingNotNull = text;
				CaretPositionBeforeTextChanging = caretPosition;
			}
		}

		/// <summary>
		/// The lower implementation of a formatting.
		/// </summary>
		/// <param name="state"></param>
		private void FormatAndManageCaret(ProcessingState state)
		{
			if (state.FormattingType == FormattingAfter.EmptyValueBeforeAndNow)
			{
				return;
			}
			if (state.FormattingType == FormattingAfter.EmptyValueBecome)
			{
				return;
			}

			JumpCaretFromPartialEndToIntegerEnd(state);

			DecimalSeparatorAlternatingReplacing(state);

			if(PartialDisabledOnInput
				&& state.PartialFormatted.IsNotNullOrEmpty())
			{
				state.Formatting.Text = state.IntegerFormatted + DecimalSeparator + ZerosPartialString;
			}
			if(PartialDisabledOnInput
				&& FocusState == FocusState.JustGotten)
			{
				state.Formatting.Text = state.IntegerFormatted;
			}

			DecimalSeparatorDeletedProcessingWithCaret(state);

			// Catch a first digital input and ingnore others.
			if(0 < state.UnformattedValue.Length
				&& state.Formatting.Text.Any(el => CustomSerialilzationChars.Contains(el)) == false)
			{
				state.Formatting.Text = String.Empty;
				state.Formatting.CaretPosition = 0;

				// !!!
				return;
			}

			DecimalSeparatorMissed(state);
			DecimalSeparatorExcessiveProcessingWithCaret(state);

			GroupSeparatorProcessingWithCaret(state);

			NotDigitCharsProcessingWithCaret(state);

			state.Formatting.Integer = state.IntegerFormatted;
			state.Formatting.Partial = state.PartialFormatted;

			IntegerPartProcessingWithCaret(state);
			PartialPartProcessingWithCaret(state);

			var preliminaryFormattedValue = state.Formatting.Integer;
			if(PartialDisabledCurrent == false)
			{
				preliminaryFormattedValue  += DecimalSeparator + state.Formatting.Partial;
			}

			if (RuntimeType == RuntimeType.Double)
			{
				state.Formatting.Text = FormatByPrecisionForDouble(preliminaryFormattedValue);
			}
			else if (RuntimeType == RuntimeType.Decimal)
			{
				state.Formatting.Text = FormatByPrecisionForDecimal(preliminaryFormattedValue);
			}
			else
			{
				WriteLogAction(() => "ERROR. Unsupported typed formatting.");

				state.Formatting.Text = preliminaryFormattedValue;
			}


			if(state.JumpCaretToEndOfInteger)
			{
				state.Formatting.CaretPosition = state.Formatting.Integer.Length;
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
			if(state.Formatting.Text.IsNullOrEmpty())
			{
				return;
			}
			if(PartialDisabledCurrent)
			{
				return;
			}
			if (state.Formatting.CaretPosition != state.Formatting.Text.Length)
			{
				return;
			}

			state.Formatting.CaretPosition = state.GetIntegerEndPosition();
		}

		/// <summary>
		/// Deleting not gigit chars with caret management.
		/// </summary>
		/// <param name="state"></param>
		private void NotDigitCharsProcessingWithCaret(ProcessingState state)
		{
			var stringForIteraction = state.Formatting.Text;
			foreach (var @char in stringForIteraction)
			{
				if (CustomSerialilzationChars.Contains(@char))
				{
					continue;
				}

				// Let's delete only the first entry of that char for now.
				var index = state.Formatting.Text.IndexOf(@char);
				state.Formatting.Text = state.Formatting.Text.Remove(index, 1);

				if (index <= (state.Formatting.CaretPosition - 1))
				{
					state.Formatting.CaretPosition -= 1;
				}
			}
		}

		private static void CorrectCaretOnEnd(ProcessingState state)
		{
			if (state.Formatting.CaretPosition < 0)
			{
				WriteLogAction(() => "!!! The negative caret position.");

				// It's definetly an error.
				// Fix the impossible negative caret position for sake of the unwanted exception.
				state.Formatting.CaretPosition = 0;
			}
			if (state.Formatting.Text.Length < state.Formatting.CaretPosition)
			{
				WriteLogAction(() => "!!! The caret position bigger than the text length.");

				// It's definetly an error.
				state.Formatting.CaretPosition = state.Formatting.Text.Length;
			}
		}

	}
}
