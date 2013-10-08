using System;
using System.Globalization;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The custom Decimal serialization.
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		private String GetCustomSerialisation(Decimal? number)
		{
			if (number == null)
			{
				return String.Empty;
			}

			var invariant = number.Value
				.ToString(NumberStandartFormattingKey, CultureInfo.InvariantCulture);

			var custom = ToCustomSerialisation(invariant);

			return custom;
		}

		/// <summary>
		/// Gets the custom Decimal conversion.
		/// </summary>
		/// <param name="customSerialisation"></param>
		/// <returns></returns>
		private Decimal? GetDecimal(String customSerialisation)
		{
			if (customSerialisation.IsNullOrEmpty())
			{
				return null;
			}

			var invariant = ToInvariantSerialisation(customSerialisation);
			WriteLogAction(() => String.Format("GetDecimal. return = {0}", invariant));

			Decimal decimalValue;
			Decimal.TryParse(
				invariant,
				NumberStyles.AllowDecimalPoint,
				CultureInfo.InvariantCulture,
				out decimalValue
				);

			return decimalValue;
		}

		/// <summary>
		/// The formatting for <see cref="Decimal"/> precision.<para/>
		/// It's the workaround for a implicit formatting by the .NET Framework numbers rounding.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private String FormatByPrecisionForDecimal(String value)
		{
			var currentDecimal = GetDecimal(value);
			var serialisationWithPrecision = GetCustomSerialisation(currentDecimal);

			return serialisationWithPrecision;
		}

	}
}
