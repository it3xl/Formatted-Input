using System;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// We must use non-breking space because of default value at the 
		/// CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.
		/// </summary>
		/// <param name="charValue"></param>
		public static void ConvertSpaceToNonBreaking(ref Char charValue)
		{
			if (charValue != BreakingSpaceChar)
			{
				return;
			}

			// It's the breaking space. Fix it!
			charValue = NonBreakingSpaceChar;
		}
	}
}
