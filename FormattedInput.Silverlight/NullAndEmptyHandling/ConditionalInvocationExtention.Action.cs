using System;
using System.Diagnostics;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	public static partial class ConditionalInvocationExtention
	{
		/// <summary>
		/// The conditial invocation of the "<see cref="action"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the Reference types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="action"/>" parameter.</param>
		/// <param name="action">A conditionally invoking parameter.</param>
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
		/// The conditial invocation of the "<see cref="action"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the Value types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="action"/>" parameter.</param>
		/// <param name="action">A conditionally invoking parameter.</param>
		[DebuggerStepThrough]
		internal static void InvokeNotDefault<TTarget>(this TTarget target, Action<TTarget> action)
			where TTarget : struct
		{
			if (target.IsDefault())
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
		/// The conditial invocation of the "<see cref="action"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the <see cref="Nullable{T}"/> types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="action"/>" parameter.</param>
		/// <param name="action">A conditionally invoking parameter.</param>
		[DebuggerStepThrough]
		internal static void InvokeNotNullFor<TTarget>(this TTarget? target, Action<TTarget> action)
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
		/// The conditial invocation of the "<see cref="action"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the <see cref="Nullable{T}"/> types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="action"/>" parameter.</param>
		/// <param name="action">A conditionally invoking parameter.</param>
		[DebuggerStepThrough]
		internal static void InvokeNotNullOrDefault<TTarget>(this TTarget? target, Action<TTarget> action)
			where TTarget : struct
		{
			if (target == null)
			{
				return;
			}
			if (target.Value.IsDefault())
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
		/// The conditial invocation of the "<see cref="action"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the String type.
		/// </summary>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="action"/>" parameter.</param>
		/// <param name="action">A conditionally invoking parameter.</param>
		[DebuggerStepThrough]
		internal static void InvokeNotNullOrEmpty(this String target, Action<String> action)
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

	}
}
