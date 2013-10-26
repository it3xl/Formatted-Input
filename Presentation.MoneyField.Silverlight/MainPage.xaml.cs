using System;
using System.Windows;
using System.Windows.Controls;
using It3xl.FormattedInput.View.Converter;
using Presentation.MoneyField.Silverlight.ViewModel;

namespace Presentation.MoneyField.Silverlight
{
	public partial class MainPage : UserControl
	{
		public ViewModelForTests TestingViewModel
		{
			get
			{
				return DataContext as ViewModelForTests;
			}
		}

		public MainPage()
		{
			NumberToMoneyConverter.ShowExeptionAction = ex => MessageBox.Show(ex.ToString());

			InitializeComponent();
		}

		private void SetAmountRandomButton_Click(object sender, RoutedEventArgs e)
		{
			var randomDouble = (new Random().NextDouble() + 1) * 80000d;

			SetValueToAll(randomDouble);
		}

		private void SetDefaultValueButton_Click(object sender, RoutedEventArgs e)
		{
			SetValueToAll(null);
		}

		private void SetValueToAll(Double? randomDouble)
		{
			TestingViewModel.DoubleNullableMoney = randomDouble;
			TestingViewModel.DoubleMoney = randomDouble ?? 0;

			TestingViewModel.DoubleMoneyTwo = randomDouble ?? 0;

			TestingViewModel.DecimalNullableMoney = randomDouble.HasValue ? (Decimal) randomDouble.Value : (Decimal?)null;
			TestingViewModel.DecimalMoney = (Decimal) (randomDouble ?? 0);
		}

	}
}
