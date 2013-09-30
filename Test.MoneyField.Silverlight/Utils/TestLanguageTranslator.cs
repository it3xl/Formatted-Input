using System;
using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{
	/// <summary>
	/// It translates common test language values to a real values for a test.
	/// </summary>
	public static class TestLanguageTranslator
	{
		private const Char DecimalSeparator = '.';
		private const Char DecimalSeparatorAlternative = '^';
		private const Char GroupSeparator = ' ';
		private const String CursorChar = "|";

		static TestLanguageTranslator()
		{
			LanguageCulture.NumberFormat
				.NumberGroupSeparator = NumberToMoneyConverter
					.NonBreakingSpaceChar.ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Converts the the common testing language to the specific value for a test.<para/>
		/// Replacements:<para/>
		/// "." - current <see cref="MoneyTextBox.DecimalSeparator"/><para/>
		/// " " - current <see cref="MoneyTextBox.GroupSeparator"/><para/>
		/// "|" - caret/cursor position mark<para/>
		/// "^" - current <see cref="MoneyTextBox.DecimalSeparatorAlternative"/><para/>
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

			result = result.Replace(DecimalSeparator, LocalScaffold.TestBoxStatic.DecimalSeparator);

			if (LocalScaffold.TestBoxStatic.DecimalSeparatorAlternative.IsDefault() == false)
			{
				result = result.Replace(DecimalSeparatorAlternative, LocalScaffold.TestBoxStatic.DecimalSeparatorAlternative);
			}

			if (LocalScaffold.TestBoxStatic.GroupSeparator.IsDefault() == false)
			{
				result = result.Replace(GroupSeparator, LocalScaffold.TestBoxStatic.GroupSeparator);
			}

			if (commonValue.Contains(CursorChar))
			{
				caretPosition = result.IndexOf(CursorChar);
				result = result.Replace(CursorChar, String.Empty);
			}

			return result;
		}

		/// <summary>
		/// The Culture Info matched to the custom test language format.
		/// </summary>
		internal static readonly CultureInfo LanguageCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

	}
}
