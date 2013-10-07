using System;
using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The custom Decimal serialization.
		/// </summary>
		/// <param name="decimalValue"></param>
		/// <returns></returns>
		private String GetCustomSerialisationFromDecimal(Decimal decimalValue)
		{
			var unformattedValue = decimalValue
				.ToString(NumberStandartFormattingKey, CultureInfo.InvariantCulture);

			// The order is important! The GroupSeparatorChar replacing must go before the DecimalSeparatorChar replacing.
			if (GroupSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				unformattedValue = unformattedValue.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator,
					GroupSeparatorChar);
			}

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				unformattedValue = unformattedValue.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator,
					DecimalSeparatorChar);
			}

			return unformattedValue;
		}

		/// <summary>
		/// The custom Decimal conversion.
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		private Decimal GetDecimalFromCustomSerialisation(String stringValue)
		{
			var cSharpDigitalSerialisation = stringValue;

			// Remove the group delimiter.
			GroupSeparator.InvokeNotDefault(el =>
			{
				if (GroupSeparator == NonBreakingSpaceChar)
				{
					cSharpDigitalSerialisation.Replace(BreakingSpaceChar, NonBreakingSpaceChar);
				}

				cSharpDigitalSerialisation = cSharpDigitalSerialisation.Replace(GroupSeparatorChar, String.Empty);
			}
			);

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn to the CultureInfo.InvariantCulture.
				cSharpDigitalSerialisation = cSharpDigitalSerialisation.Replace(
					DecimalSeparatorChar,
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
			}

			WriteLogAction(() => String.Format("ConvertBack. return = {0}", cSharpDigitalSerialisation));

			Decimal decimalValue;
			Decimal.TryParse(
				cSharpDigitalSerialisation,
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
			var currentDecimal = GetDecimalFromCustomSerialisation(value);
			var serialisationWithPrecision = GetCustomSerialisationFromDecimal(currentDecimal);

			return serialisationWithPrecision;
		}

	}
}
