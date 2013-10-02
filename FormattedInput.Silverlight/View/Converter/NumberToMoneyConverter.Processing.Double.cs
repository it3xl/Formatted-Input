using System;
using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The custom double serialization.
		/// </summary>
		/// <param name="doubleValue"></param>
		/// <returns></returns>
		private String GetCustomSerialisationFromDouble(Double doubleValue)
		{
			var unformattedValue = doubleValue
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
		/// The custom double conversion.
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		private double GetDoubleFromCustomSerialisation(String stringValue)
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

			Double doubleValue;
			Double.TryParse(
				cSharpDigitalSerialisation,
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
			var currentDouble = GetDoubleFromCustomSerialisation(value);
			var serialisationWithPrecision = GetCustomSerialisationFromDouble(currentDouble);

			return serialisationWithPrecision;
		}

	}
}
