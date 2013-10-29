using System;
using System.Globalization;
using System.Linq;
using It3xl.FormattedInput.NullAndEmptyHandling;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// Converts the invariant .NET Framework serialisation to a custom number serialisation.
		/// </summary>
		/// <param name="invariantSerialisation"></param>
		/// <returns></returns>
		private String ToCustomSerialisation(String invariantSerialisation)
		{
			var customSerialisation = invariantSerialisation;

			if (PartialDisabledCurrent)
			{
				customSerialisation =
					customSerialisation.Split(CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator.ToCharFirst()).First();
			}

			// The order is important! The GroupSeparatorChar replacing must go before the DecimalSeparatorChar replacing.
			if (GroupSeparatorCharDyninic != CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				customSerialisation = customSerialisation.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator,
					GroupSeparatorCharDyninic);
			}

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn form CultureInfo.InvariantCulture to the custom separator.
				customSerialisation = customSerialisation.Replace(
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator,
					DecimalSeparatorChar);
			}
			return customSerialisation;
		}

		/// <summary>
		/// Converts a custom number serialisation to the invariant .NET Framework serialisation.
		/// </summary>
		/// <param name="customSerialisation"></param>
		/// <returns></returns>
		private string ToInvariantSerialisation(string customSerialisation)
		{
			var invariantSerialisation = customSerialisation;

			// Remove the group delimiter.
			GroupSeparator.InvokeNotDefault(el =>
			{
				if (GroupSeparator == NonBreakingSpaceChar)
				{
					invariantSerialisation = invariantSerialisation.Replace(BreakingSpaceChar, NonBreakingSpaceChar);
				}

				invariantSerialisation = invariantSerialisation.Replace(GroupSeparatorChar, String.Empty);
			}
				);

			if (DecimalSeparatorChar != CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
			{
				// The turn to the CultureInfo.InvariantCulture.
				invariantSerialisation = invariantSerialisation.Replace(
					DecimalSeparatorChar,
					CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator);
			}

			return invariantSerialisation;
		}

	}
}
