using System;
using System.Diagnostics;

namespace MoneyField.Silverlight.NullAndEmptyHandling
{
	/// <summary>
	/// Расширения, позволяющие выполнять действие только для не нулевых объектов и структур не равных значению по умлочанию.
	/// </summary>
	public static class NotNullNotDefaultExecutionExtention
	{



		// Для строки сделать отдельный метод Not Null + Not Empty.

		// Для строки сделать Is null or empty.















		/// <summary>
		/// Executes the <see cref="getter"/> Func if the <see cref="target"/> is not null.
		/// Or returns default type value.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="target"></param>
		/// <param name="getter"></param>
		/// <returns></returns>
		public static TValue GetSafe<TTarget, TValue>(this TTarget target, Func<TTarget, TValue> getter)
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
		/// <typeparam name="TValue"></typeparam>
		/// <param name="target"></param>
		/// <param name="setter">The some property setter.</param>
		/// <returns></returns>
		public static void SetSafe<TTarget>(this TTarget target, Action<TTarget> setter)
			where TTarget : class 
		{
			target.InvokeNotNull(setter);
		}


		/// <summary>
		/// Выполнит действие, если параметр не ссылается на null.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		public static void InvokeNotNull<TTarget>(this TTarget target, Action<TTarget> action)
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
		/// Выполнит действие, если параметр не ссылается на null.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		public static void InvokeNotNullNotEmpty(this String target, Action<String> action)
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
		/// Выполнит дейстие, если значение параметра не указывает на null.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		public static void InvokeNotNull<TTarget>(this TTarget? target, Action<TTarget> action)
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
		/// Выполнит дейстие, если значение параметра типа значения не указывает на значение типа по умолчанию.
		/// </summary>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="target"></param>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		public static void InvokeIfNotDefault<TTarget>(this TTarget target, Action<TTarget> action)
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


	}
}
