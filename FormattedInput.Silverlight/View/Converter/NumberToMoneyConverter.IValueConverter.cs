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
			WriteLogAction(() => String.Format("* Convert/{0} (-> Process' caret will be ignored)", parameter));

			var doubleNullableValue = value as Double?;
			var decimalNullableValue = value as Decimal?;

			if (doubleNullableValue == null && decimalNullableValue == null)
			{
				WriteLogAction(() => String.Format("return = {0}", "null"));

				return String.Empty;
			}

			String unformattedValue;
			if(doubleNullableValue.HasValue)
			{
				unformattedValue = GetCustomSerialisation(doubleNullableValue.Value);
				SetViewModelValueChanged(doubleNullableValue);
				SetRuntimeType(RuntimeType.Double);
			}
			else if(decimalNullableValue.HasValue)
			{
				unformattedValue = GetCustomSerialisation(decimalNullableValue.Value);
				SetViewModelValueChanged(decimalNullableValue);
				SetRuntimeType(RuntimeType.Decimal);
			}
			else
			{
				WriteLogAction(() => String.Format("Unsupported data type - {0}", value.GetType().Name));

				return String.Empty;
			}

			String formatteValue;
			var caretPositionDummy = 0;

			Process(unformattedValue, out formatteValue, ref caretPositionDummy);
			// !!! caretPositionDummy
			// it has no sense to set the caret position here as a target MoneyTextBox may have a different value in many cases.
			// If length of a current text will be shorter than a caret position, then the caret position will be ignored.
			// And all things are ignored if a debugger not attached (Debugger.IsAttached == false).
			//_moneyBox.ExecuteIfTargetNotNull(el => el.SelectionStart = caretPositionDummy);

			WriteLogAction(() => String.Format("formattedValue = {0}. unformattedValue = {1}", formatteValue, unformattedValue));

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
			WriteLogAction(() => String.Format("* ConvertBack/{0}", parameter));

			SetRuntimeType(targetType);

			Object convertBack = GetTypeDefaultValue(targetType);

			var stringValue = value as String;
			if (String.IsNullOrEmpty(stringValue))
			{
				WriteLogAction(() => String.Format("return {0}", "null"));

				return convertBack;
			}

			if (GetConvertedValue(targetType, stringValue, ref convertBack))
			{
				return convertBack;
			}

			SetLastViewModelValue(null, null);

			return convertBack;
		}

		private bool GetConvertedValue(Type targetType, string stringValue, ref object convertBack)
		{
			if (targetType == _typeDouble
			    || targetType == _typeDoubleNullabe)
			{
				var doubleValue = GetDouble(stringValue);
				SetLastViewModelValue(doubleValue, null);
				convertBack = doubleValue;

				return true;
			}

			if (targetType == _typeDecimal
			    || targetType == _typeDecimalNullabe)
			{
				var decimalValue = GetDecimal(stringValue);
				SetLastViewModelValue(null, decimalValue);
				convertBack = decimalValue;

				return true;
			}

			return false;
		}

		/// <summary>
		/// Gets a default value of a type.
		/// </summary>
		/// <param name="targetType"></param>
		/// <returns></returns>
		private Object GetTypeDefaultValue(Type targetType)
		{
			if(targetType == _typeDouble || targetType == _typeDecimal)
			{
				return 0;
			}

			return null;
		}


		private void SetViewModelValueChanged(Double? viewModelDouble)
		{
			ViewModelValueChanged = _lastViewDouble != viewModelDouble;
		}

		private void SetViewModelValueChanged(Decimal? viewModelDecimal)
		{
			ViewModelValueChanged = _lastViewDecimal != viewModelDecimal;
		}

		private void SetLastViewModelValue(Double? viewDouble, Decimal? viewDecimal)
		{
			_lastViewDouble = viewDouble;
			_lastViewDecimal = viewDecimal;
		}

		/// <summary>
		/// Sets current typed formatting.
		/// </summary>
		/// <param name="targetType"></param>
		private void SetRuntimeType(RuntimeType targetType)
		{
			RuntimeType = targetType;
		}

		/// <summary>
		/// Sets current typed formatting.
		/// </summary>
		/// <param name="targetType"></param>
		private void SetRuntimeType(Type targetType)
		{
			if (targetType == _typeDouble
				|| targetType == _typeDoubleNullabe)
			{
				RuntimeType = RuntimeType.Double; ;
			}

			if (targetType == _typeDecimal
				|| targetType == _typeDecimalNullabe)
			{
				RuntimeType = RuntimeType.Decimal; ;
			}

			RuntimeType = RuntimeType.NotInitialized;
		}

	}
}
