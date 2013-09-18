using System;

namespace MoneyField.Silverlight.NullAndEmptyHandling
{
	/// <summary>
	/// The implementation of the Managed Closure Pattern.<para/>
	/// https://managedclosure.codeplex.com/
	/// </summary>
	/// <remarks>
	/// TODO.it3xl.com: В следующих классах разобраться с причиной обнуления целевого объекта и заменить там этот класс на <see cref="WeakClosure{T}"/>.
	/// Web\Calculation\CreditCalculation\Controller\QuickYes\CalculatorQuickYesControllerBase.cs
	/// + PopUp открываемые из калькуляторов (см. мои туду).
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class ManagedClosure<T> : WeakClosure<T>
		where T : class
	{
		public ManagedClosure(T target)
			: base(target)
		{
			StrongReference = target;
		}

		public ManagedClosure(T target, Boolean trackResurrection)
			: base(target, trackResurrection)
		{
			StrongReference = target;
		}


		/// <summary>
		/// 
		/// </summary>
		private Object StrongReference { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Boolean StrongReferenceRelease
		{
			get
			{
				return StrongReference == null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void ReleaseStrongReference()
		{
			StrongReference = null;
		}

	}
}
