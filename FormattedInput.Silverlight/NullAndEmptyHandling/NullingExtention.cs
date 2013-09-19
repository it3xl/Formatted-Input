using System;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	public static class NullingExtention
	{
		public static Boolean IsNullOrEmpty(this String val)
		{
			return String.IsNullOrEmpty(val);
		}
	}
}
