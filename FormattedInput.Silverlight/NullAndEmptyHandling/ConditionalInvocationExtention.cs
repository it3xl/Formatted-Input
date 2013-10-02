using System;
using System.Diagnostics;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	/// <summary>
	/// The extension for conditional invocations. 
	/// </summary>
	public static partial class ConditionalInvocationExtention
	{
	
		// TODO.it3xl.com: Move it to the target project.

		/// <summary>
		/// Defines that parameter equals to its type's default value.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		[DebuggerStepThrough]
		public static Boolean IsDefault<TTarget>(this TTarget target)
			where TTarget : struct
		{
			return Equals(target, default(TTarget));
		}

	}
}
