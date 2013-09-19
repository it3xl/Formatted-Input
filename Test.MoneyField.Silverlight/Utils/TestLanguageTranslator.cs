using System;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{
	/// <summary>
	/// It translates common test language values to a real values for a test.
	/// </summary>
	public static class TestLanguageTranslator
	{
		/// <summary>
		/// Converts the the common testing language to the specific value for a test.<para/>
		/// Replacements:<para/>
		/// "." - current <see cref="MoneyTextBox.DecimalSeparator"/><para/>
		/// " " - current <see cref="MoneyTextBox.GroupSeparator"/><para/>
		/// "|" - caret/cursor position mark<para/>
		/// "." - current <see cref="MoneyTextBox.AlternativeDecimalSeparator"/><para/>
		/// </summary>
		/// <param name="commonValue"></param>
		/// <param name="caretPosition">The result caret position.</param>
		/// <returns></returns>
		public static String ToSpecificValue(this String commonValue, out Int32 caretPosition)
		{
			caretPosition = Int32.MinValue;
			if (commonValue.IsNullOrEmpty())
			{
				return commonValue;
			}

			var result = commonValue;

			result = result.Replace('.', LocalScaffold.TestBoxStatic.DecimalSeparator);

			if (LocalScaffold.TestBoxStatic.AlternativeDecimalSeparator.IsDefault() == false)
			{
				result = result.Replace('_', LocalScaffold.TestBoxStatic.AlternativeDecimalSeparator);
			}

			if (LocalScaffold.TestBoxStatic.GroupSeparator.IsDefault() == false)
			{
				result = result.Replace(' ', LocalScaffold.TestBoxStatic.GroupSeparator);
			}

			if (commonValue.Contains("|"))
			{
				caretPosition = result.IndexOf('|');
				result = result.Replace("|", String.Empty);
			}

			return result;
		}

	}
}
