using System;
using System.Diagnostics;

namespace It3xl.FormattedInput.NullAndEmptyHandling
{
	public static partial class ConditionalInvocationExtension
	{
		/// <summary>
		/// The conditial invocation of the "<see cref="func"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the Reference types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <typeparam name="TResult">A returned type.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="func"/>" parameter.</param>
		/// <param name="func">A conditionally invoking parameter.</param>
		/// <returns>
		/// In accordance with the condition in the name of this method:<para/>
		/// true: will be returned a result of an invocation of the "<see cref="func"/>".<para/>
		/// false: will be returned the "default(<see cref="TResult"/>)".
		/// </returns>
		[DebuggerStepThrough]
		internal static TResult InvokeNotNull<TTarget, TResult>(this TTarget target, Func<TTarget, TResult> func)
			where TTarget : class
		{
			if (target == null)
			{
				return default(TResult);
			}

			if (func == null)
			{
				return default(TResult);
			}

			return func(target);
		}

		/// <summary>
		/// The conditial invocation of the "<see cref="func"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the Value types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <typeparam name="TResult">A returned type.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="func"/>" parameter.</param>
		/// <param name="func">A conditionally invoking parameter.</param>
		/// <returns>
		/// In accordance with the condition in the name of this method:<para/>
		/// true: will be returned a result of an invocation of the "<see cref="func"/>".<para/>
		/// false: will be returned the "default(<see cref="TResult"/>)".
		/// </returns>
		[DebuggerStepThrough]
		internal static TResult InvokeNotDefault<TTarget, TResult>(this TTarget target, Func<TTarget, TResult> func)
			where TTarget : struct
		{
			if (target.IsDefault())
			{
				return default(TResult);
			}

			if (func == null)
			{
				return default(TResult);
			}

			return func(target);
		}

		/// <summary>
		/// The conditial invocation of the "<see cref="func"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the <see cref="Nullable{T}"/> types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <typeparam name="TResult">A returned type.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="func"/>" parameter.</param>
		/// <param name="func">A conditionally invoking parameter.</param>
		/// <returns>
		/// In accordance with the condition in the name of this method:<para/>
		/// true: will be returned a result of an invocation of the "<see cref="func"/>".<para/>
		/// false: will be returned the "default(<see cref="TResult"/>)".
		/// </returns>
		[DebuggerStepThrough]
		internal static TResult InvokeNotNullFor<TTarget, TResult>(this TTarget? target, Func<TTarget, TResult> func)
			where TTarget : struct
		{
			if (target == null)
			{
				return default(TResult);
			}

			if (func == null)
			{
				return default(TResult);
			}

			return func(target.Value);
		}

		/// <summary>
		/// The conditial invocation of the "<see cref="func"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the <see cref="Nullable{T}"/> types.
		/// </summary>
		/// <typeparam name="TTarget">A target type for the conditional invocation.</typeparam>
		/// <typeparam name="TResult">A returned type.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="func"/>" parameter.</param>
		/// <param name="func">A conditionally invoking parameter.</param>
		/// <returns>
		/// In accordance with the condition in the name of this method:<para/>
		/// true: will be returned a result of an invocation of the "<see cref="func"/>".<para/>
		/// false: will be returned the "default(<see cref="TResult"/>)".
		/// </returns>
		[DebuggerStepThrough]
		internal static TResult InvokeNotNullOrDefault<TTarget, TResult>(this TTarget? target, Func<TTarget, TResult> func)
			where TTarget : struct
		{
			if (target == null)
			{
				return default(TResult);
			}
			if (target.Value.IsDefault())
			{
				return default(TResult);
			}

			if (func == null)
			{
				return default(TResult);
			}

			return func(target.Value);
		}

		/// <summary>
		/// The conditial invocation of the "<see cref="func"/>" parameter depending on the "<see cref="target"/>" parameter.<para/>
		/// For the String type.
		/// </summary>
		/// <typeparam name="TResult">A returned type.</typeparam>
		/// <param name="target">A parameter for the conditional invocation and passing it to the "<see cref="func"/>" parameter.</param>
		/// <param name="func">A conditionally invoking parameter.</param>
		/// <returns>
		/// In accordance with the condition in the name of this method:<para/>
		/// true: will be returned a result of an invocation of the "<see cref="func"/>".<para/>
		/// false: will be returned the "default(<see cref="String"/>)".
		/// </returns>
		[DebuggerStepThrough]
		internal static TResult InvokeNotNullOrEmpty<TResult>(this String target, Func<String, TResult> func)
		{
			if (String.IsNullOrEmpty(target))
			{
				return default(TResult);
			}

			if (func == null)
			{
				return default(TResult);
			}

			return func(target);
		}

	}
}
