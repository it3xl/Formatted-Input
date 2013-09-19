using System;
using MoneyField.Silverlight.View;

namespace TestMoneyFieldSilverlight.Utils
{
	using MoneyField.Silverlight.NullAndEmptyHandling;

	/// <summary>
	/// It translates common test values to a specific mode values.
	/// </summary>
	public static class CommonValueTranslator
	{
		/// <summary>
		/// Converts the the common testing language to the specific value.<para/>
		/// Replacements:<para/>
		/// "." - current <see cref="MoneyTextBox.DecimalSeparator"/><para/>
		/// " " - current <see cref="MoneyTextBox.GroupSeparator"/><para/>
		/// "." - current <see cref="MoneyTextBox.AlternativeDecimalSeparator"/><para/>
		/// </summary>
		/// <param name="commonValue"></param>
		/// <returns></returns>
		public static String ToSpecificValue(this String commonValue)
		{
			if (commonValue.IsNullOrEmpty())
			{
				return commonValue;
			}

			var result = commonValue;

			result = result.Replace('.', Scaffold.TestBoxStatic.DecimalSeparator);

			if (Scaffold.TestBoxStatic.AlternativeDecimalSeparator.IsDefault() == false)
			{
				result = result.Replace('_', Scaffold.TestBoxStatic.AlternativeDecimalSeparator);
			}

			if (Scaffold.TestBoxStatic.GroupSeparator.IsDefault() == false)
			{
				result = result.Replace(' ', Scaffold.TestBoxStatic.GroupSeparator);
			}

			return result;
		}

	}
}
