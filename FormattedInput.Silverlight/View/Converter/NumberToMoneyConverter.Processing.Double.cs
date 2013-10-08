using System;
using System.Globalization;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The custom double serialization.
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		private String GetCustomSerialisation(Double? number)
		{
			if(number == null)
			{
				return String.Empty;
			}

			var invariant = number.Value
				.ToString(NumberStandartFormattingKey, CultureInfo.InvariantCulture);

			var custom = ToCustomSerialisation(invariant);

			return custom;
		}

		/// <summary>
		/// Gets the custom double conversion.
		/// </summary>
		/// <param name="customSerialisation"></param>
		/// <returns></returns>
		private Double? GetDouble(String customSerialisation)
		{
			if (customSerialisation.IsNullOrEmpty())
			{
				return null;
			}

			var invariant = ToInvariantSerialisation(customSerialisation);
			WriteLogAction(() => String.Format("GetDouble. return = {0}", invariant));

			Double doubleValue;
			Double.TryParse(
				invariant,
				NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture,
				out doubleValue
				);

			return doubleValue;
		}

		/// <summary>
		/// The formatting for <see cref="Double"/> precision.<para/>
		/// It's the workaround for a implicit formatting by the .NET Framework numbers rounding.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private String FormatByPrecisionForDouble(String value)
		{
			var currentDouble = GetDouble(value);
			var serialisationWithPrecision = GetCustomSerialisation(currentDouble);

			return serialisationWithPrecision;
		}

	}
}
