using System;

namespace MoneyField.Silverlight.NullAndEmptyHandling
{
	public static class NullingExtention
	{
		public static Boolean IsNullOrEmpty(this String val)
		{
			return String.IsNullOrEmpty(val);
		}
	}
}
