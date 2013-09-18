using System;

namespace TestMoneyFieldSilverlight.Utils
{
	using MoneyField.Silverlight.NullAndEmptyHandling;

	/// <summary>
	/// It translates common test values to a specific mode values.
	/// </summary>
	public static class CommonValueTranslator
	{
		public static String ToSpecificValue(this String commonValue)
		{
			if (commonValue.IsNullOrEmpty())
			{
				return commonValue;
			}

			return commonValue
				.Replace('.', Scaffold.TestBoxStatic.DecimalSeparator)
				.Replace(' ', Scaffold.TestBoxStatic.GroupSeparator)
				;
		}

	}
}
