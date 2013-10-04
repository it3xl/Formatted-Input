using System;
using System.Linq;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	public static class CharExtension
	{
		/// <summary>
		/// Converts the String type to the Char from the first simbol of the String.
		/// </summary>
		/// <param name="targer"></param>
		/// <returns></returns>
		public static char ToCharFromFirst(this String targer)
		{
			if(targer.IsNullOrEmpty())
			{
				return Char.MinValue;
			}

			return targer.First();
		}
	}
}
