using System;
using Microsoft.Practices.Prism.ViewModel;

namespace It3xl.Scaffold.MoneyField.Silverlight.ViewModel
{
	/// <summary>
	/// The testing ViewModel.
	/// </summary>
	public class ViewModelForTests : NotificationObject
	{
		private Double? _amountDouble;
		public Double? AmountDouble
		{
			get
			{
				return _amountDouble;
			}
			set
			{
				_amountDouble = value;

				RaisePropertyChanged(() => AmountDouble);
			}
		}


		private Decimal? _amountDecimal;
		public Decimal? AmountDecimal
		{
			get
			{
				return _amountDecimal;
			}
			set
			{
				_amountDecimal = value;

				RaisePropertyChanged(() => AmountDecimal);
			}
		}

	
	}
}