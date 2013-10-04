using System;
using Microsoft.Practices.Prism.ViewModel;

namespace It3xl.Scaffold.MoneyField.Silverlight.ViewModel
{
	/// <summary>
	/// The testing ViewModel.
	/// </summary>
	public class ViewModelForTests : NotificationObject
	{
		private Double? _doubleNullableMoney;
		public Double? DoubleNullableMoney
		{
			get
			{
				return _doubleNullableMoney;
			}
			set
			{
				_doubleNullableMoney = value;

				RaisePropertyChanged(() => DoubleNullableMoney);
			}
		}

		private Double _doubleMoney;
		public Double DoubleMoney
		{
			get
			{
				return _doubleMoney;
			}
			set
			{
				_doubleMoney = value;

				RaisePropertyChanged(() => DoubleMoney);
			}
		}


		private Decimal? _decimalNullableMoney;
		public Decimal? DecimalNullableMoney
		{
			get
			{
				return _decimalNullableMoney;
			}
			set
			{
				_decimalNullableMoney = value;

				RaisePropertyChanged(() => DecimalNullableMoney);
			}
		}

		private Decimal _decimalMoney;
		public Decimal DecimalMoney
		{
			get
			{
				return _decimalMoney;
			}
			set
			{
				_decimalMoney = value;

				RaisePropertyChanged(() => DecimalMoney);
			}
		}

	
	}
}