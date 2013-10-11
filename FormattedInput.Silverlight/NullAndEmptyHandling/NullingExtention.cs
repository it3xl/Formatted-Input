using System;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	public static class NullingExtention
	{
		public static Boolean IsNullOrEmpty(this String val)
		{
			return String.IsNullOrEmpty(val);
		}

		public static Boolean IsNotNullOrEmpty(this String val)
		{
			return String.IsNullOrEmpty(val) == false;
		}

		public static Int32 LengthNullable(this String val)
		{
			if(val.IsNullOrEmpty())
			{
				return 0;
			}
			return val.Length;
		}
	}
}
