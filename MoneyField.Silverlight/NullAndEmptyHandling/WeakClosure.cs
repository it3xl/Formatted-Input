using System;
using System.Diagnostics;

namespace MoneyField.Silverlight.NullAndEmptyHandling
{
	/// <summary>
	/// The implementation of the Weak Closure Pattern.
	/// Use the <see cref="Nullable{T}"/> if you want to work with the value type of <see cref="T"/>
	/// <para/>
	/// If you used to use post-actions with the closures and guaranteed don't want to get problems with memory leaks.
	/// 
	/// Это позволяет упростить объекты потребители, которые не будут должны реализовывать безопасное для утечек хранение переданных действий.<para/>
	/// Но это усложняет код объектов поставщиков таких действий.<para/>
	/// </summary>
	/// <typeparam name="T">A type of a target object catched by the closure.</typeparam>
	/// <remarks>
	/// https://weakclosure.codeplex.com/
	/// </remarks>
	/// <example>
	/// 
	/// </example>
	public class WeakClosure<T> : WeakReference<T>
		where T : class
	{
		public WeakClosure(T target) : base(target)
		{
		}

		public WeakClosure(T target, Boolean trackResurrection)
			: base(target, trackResurrection)
		{
		}

		/// <summary>
		/// Удобный метод, позволяющий сократить код за счет инкапсуляции в нем проверок на null целевого объекта.<para/>
		/// !!! Don't create any closure at the <see cref="action"/>.<para/>
		/// Use the <see cref="action"/> parameter only.<para/>
		/// <para/>
		/// Если вы сомневаетесь или не знаете, что такое замыкание, то не используйте этот метод!
		/// Или используйте обычную проверку на if(<see cref="WeakReference{T}.TargetTyped"/> != null).
		/// </summary>
		/// <param name="action"></param>
		[DebuggerStepThrough]
		public void ExecuteIfTargetNotNull(Action<T> action)
		{
			var targetObject = TargetTyped;
			if (targetObject == null)
			{
				return;
			}

			action(targetObject);
		}
	}
}
