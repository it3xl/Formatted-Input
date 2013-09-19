using System;
using System.Diagnostics;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	/// <summary>
	/// Расширения, позволяющие выполнять действие только для не нулевых объектов и структур не равных значению по умлочанию.
	/// Processing of the null or default values.
	/// </summary>
	public static class NotNullNotDefaultExecutionExtention
	{
		/// <summary>
		/// Executes the <see cref="getter"/> Func if the <see cref="target"/> is not null.
		/// Or returns default type value.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="target"></param>
		/// <param name="getter"></param>
		/// <returns></returns>
		internal static TValue GetSafe<TTarget, TValue>(this TTarget target, Func<TTarget, TValue> getter)
			where TTarget : class 
		{
			if (Equals(target, default(TTarget)))
			{
				return default(TValue);
			}

			if (getter == null)
			{
				return default(TValue);
			}

			return getter(target);
		}


		/// <summary>
		/// Executes the <see cref="setter"/> Action if the <see cref="target"/> is not null.
		/// Or returns default type value.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="setter">The some property setter.</param>
		/// <returns></returns>
		internal static void SetSafe<TTarget>(this TTarget target, Action<TTarget> setter)
			where TTarget : class 
		{
			target.InvokeNotNull(setter);
		}


		/// <summary>
		/// Executes the action if the parameter is not null.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action">Input action.</param>
		[DebuggerStepThrough]
		internal static void InvokeNotNull<TTarget>(this TTarget target, Action<TTarget> action)
			where TTarget : class 
		{
			if (target == null)
			{
				return;
			}

			if (action == null)
			{
				return;
			}

			action(target);
		}


		/// <summary>
		/// Executes the action if the parameter is not null.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		internal static void InvokeNotNullNotEmpty(this String target, Action<String> action)
		{
			if (String.IsNullOrEmpty(target))
			{
				return;
			}

			if (action == null)
			{
				return;
			}

			action(target);
		}

		/// <summary>
		/// Executes the action if the parameter is not null.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		internal static void InvokeNotNull<TTarget>(this TTarget? target, Action<TTarget> action)
			where TTarget : struct 
		{
			if (target == null)
			{
				return;
			}

			if (action == null)
			{
				return;
			}

			action(target.Value);
		}

		/// <summary>
		/// Executes the action if the parameter not equals to its type default value.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		internal static void InvokeIfNotDefault<TTarget>(this TTarget target, Action<TTarget> action)
			where TTarget : struct
		{
			if (Equals(target, default(TTarget)))
			{
				return;
			}

			if (action == null)
			{
				return;
			}

			action(target);
		}

		/// <summary>
		/// Defines that parameter equals to its type default value.
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
