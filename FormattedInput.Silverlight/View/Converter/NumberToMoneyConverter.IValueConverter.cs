using System;
using System.Globalization;
using System.Windows.Data;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter : IValueConverter
	{
		/// <summary>
		/// Converts from a ViewModel to a View.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var doubleNullableValue = value as Double?;

			if (doubleNullableValue == null)
			{
				WriteLogAction(() => String.Format("Convert. return = {0}", "null"));

				return String.Empty;
			}

			var doubleValue = doubleNullableValue.Value;

			var unformattedValue = GetCustomSerialisationFromDouble(doubleValue);

			String formatteValue;
			var selectionStartDummy = 0;
			FormatAndManageCaret(unformattedValue, null, 0, out formatteValue, ref selectionStartDummy);

			WriteLogAction(() => String.Format("Convert. unformattedValue = {0}", unformattedValue));
			WriteLogAction(() => String.Format("Convert. formattedValue = {0}", formatteValue));

			return formatteValue;
		}


		/// <summary>
		/// Converts from a View to a ViewModel.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var stringValue = value as String;
			if (String.IsNullOrEmpty(stringValue))
			{
				WriteLogAction(() => String.Format("ConvertBack. return = {0}", "null"));

				// Only a string is available.
				return null;
			}

			var doubleValue = GetDoubleFromCustomSerialisation(stringValue);

			return doubleValue;
		}
	}
}
