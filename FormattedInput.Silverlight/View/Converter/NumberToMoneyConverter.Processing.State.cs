using It3xl.FormattedInput.View.Controller;

namespace It3xl.FormattedInput.View.Converter
{
	using StateController = ProcessingState.StateController;

	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Prepares states for current processing.
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="caretPosition"></param>
		/// <param name="lastCaretPosition"></param>
		/// <returns></returns>
		private ProcessingState PrepareStates(string unformattedValue, int caretPosition, int lastCaretPosition)
		{
			var controller = new StateController(
				DecimalSeparator,
				PartialDisabledCurrent,
				GroupSeparator,
				TextBeforeChangingNotNull
				);

			var jumpCaretToEndOfInteger = JumpCaretToEndOfIntegerOnNextProcessing;

			var state = controller.GetProcessingStates(
				lastCaretPosition,
				FocusState,
				caretPosition,
				unformattedValue,
				jumpCaretToEndOfInteger
				);

			// It's a crappy trick but I should play by rules for now (because i tired).
			JumpCaretToEndOfIntegerOnNextProcessing = false;
			if (ViewModelValueChanged)
			{
				ViewModelValueChanged = false;
				JumpCaretToEndOfIntegerOnNextProcessing = true;
			}

			return state;
		}
	
	}
}
