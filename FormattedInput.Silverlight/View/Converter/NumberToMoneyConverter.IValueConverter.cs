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
			var decimalNullableValue = value as Decimal?;

			if (doubleNullableValue == null && decimalNullableValue == null)
			{
				WriteLogAction(() => String.Format("Convert. return = {0}", "null"));

				return String.Empty;
			}

			String unformattedValue;
			if(doubleNullableValue.HasValue)
			{
				unformattedValue = GetCustomSerialisation(doubleNullableValue.Value);
			}
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			// ReSharper disable HeuristicUnreachableCode
			else if(decimalNullableValue.HasValue)
			{
				unformattedValue = GetCustomSerialisation(decimalNullableValue.Value);
			}
			else
			{
				WriteLogAction(() => String.Format("Convert. Unsupported data type - {0}", value.GetType().Name));

				return String.Empty;
			}
			// ReSharper restore ConditionIsAlwaysTrueOrFalse
			// ReSharper restore HeuristicUnreachableCode

			String formatteValue;
			var selectionStartDummy = 0;

			Process(unformattedValue, out formatteValue, ref selectionStartDummy);

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
				WriteLogAction(() => String.Format("ConvertBack. return {0}", "null"));

				// Only a string is available.
				return null;
			}

			if(targetType == _typeDouble 
				|| targetType == _typeDoubleNullabe)
			{
				var doubleValue = GetDouble(stringValue);

				return doubleValue;
			}

			if(targetType == _typeDecimal 
				|| targetType == _typeDecimalNullabe)
			{
				var decimalValue = GetDecimal(stringValue);

				return decimalValue;
			}

			return null;
		}
	}
}
