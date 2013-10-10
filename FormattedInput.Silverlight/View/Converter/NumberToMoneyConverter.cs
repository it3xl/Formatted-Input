using System;
using WeakClosureProject;

namespace It3xl.FormattedInput.View.Converter
{
	public sealed partial class NumberToMoneyConverter
	{
		/// <summary>
		/// The private constructor that blocks the XAML using for sake of an encapsulation.<para/>
		/// Don't use it!
		/// </summary>
		private NumberToMoneyConverter()
		{
			throw new NotImplementedException("You should use the parameterized constructor.");
		}

		/// <summary>
		/// The primary constructor.
		/// </summary>
		/// <param name="moneyBox"></param>
		public NumberToMoneyConverter(MoneyTextBox moneyBox)
		{
			_moneyBox = new WeakClosure<MoneyTextBox>(moneyBox);
		}
	}
}
