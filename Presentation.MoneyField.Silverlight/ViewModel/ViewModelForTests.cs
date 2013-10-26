using System;
using It3xl.FormattedInput.View.Converter;
using Microsoft.Practices.Prism.ViewModel;

namespace Presentation.MoneyField.Silverlight.ViewModel
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


		private Double _doubleMoneyTwo;
		public Double DoubleMoneyTwo
		{
			get
			{
				return _doubleMoneyTwo;
			}
			set
			{
				_doubleMoneyTwo = value;
				RaisePropertyChanged(() => DoubleMoneyTwo);
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