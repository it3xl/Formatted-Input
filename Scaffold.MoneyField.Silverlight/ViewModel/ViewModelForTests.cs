using System;
using It3xl.FormattedInput.View.Converter;
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

				NumberToMoneyConverter.WriteLogAction(() => String.Format("  vm  DoubleNullableMoney = {0}", value));
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

				NumberToMoneyConverter.WriteLogAction(() => String.Format("  vm  DoubleMoney = {0}", value));
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

				NumberToMoneyConverter.WriteLogAction(() => String.Format("  vm  DecimalNullableMoney = {0}", value));
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

				NumberToMoneyConverter.WriteLogAction(() => String.Format("  vm  DecimalMoney = {0}", value));
				RaisePropertyChanged(() => DecimalMoney);
			}
		}

	
	}
}