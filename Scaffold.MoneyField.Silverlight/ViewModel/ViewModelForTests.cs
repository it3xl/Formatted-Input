using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.Prism.ViewModel;

namespace It3xl.Scaffold.MoneyField.Silverlight.ViewModel
{
	/// <summary>
	/// The testing ViewModel.
	/// </summary>
	public class ViewModelForTests : NotificationObject, INotifyDataErrorInfo
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
				//RaiseErrorsChanged("LoanAmount");
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


















		public IEnumerable GetErrors(string propertyName)
		{
			var result = new List<String>();

			return result;
		}

		private bool _hasErrors;
		public bool HasErrors
		{
			get
			{
				return _hasErrors;
			}
		}

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		private void RaiseErrorsChanged(string propertyName)
		{
			var handler = ErrorsChanged;
			if (handler == null)
			{
				return;
			}
			handler(this, new DataErrorsChangedEventArgs(propertyName));
		}

	
	
	
	
	
	}
}