﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;

namespace It3xl.Scaffold.MoneyField.Silverlight
{
	public partial class MainPage
	{
		public MoneyTextBox DoubleNullableMoneyTexBox { get; private set; }
		public MoneyTextBox DoubleMoneyTexBox { get; private set; }
		public MoneyTextBox DecimalNullableMoneyTexBox { get; private set; }
		public MoneyTextBox DecimalMoneyTexBox { get; private set; }

		public ViewModelForTests TestingViewModel
		{
			get
			{
				return DataContext as ViewModelForTests;
			}
		}


		private static ItemsControl _logItemsControl;
		private static ObservableCollection<String> LogItems
		{
			get
			{
				return _logItemsControl.ItemsSource as ObservableCollection<String>;
			}
		}

		private static DateTime _lastLogTime = DateTime.Now;

		private static void AddLogItem(Func<String> logMessageAction)
		{
			String logMessage = logMessageAction();

			var span = DateTime.Now - _lastLogTime;
			if (LogItems.Any()
				&& (TimeSpan.FromMilliseconds(100) < span))
			{
				LogItems.Insert(0, "_");
			}
			_lastLogTime = DateTime.Now;

			LogItems.Insert(0, logMessage);
		}


		public MainPage()
		{
			NumberToMoneyConverter.ShowExeptionAction = ex => MessageBox.Show(ex.ToString());
			NumberToMoneyConverter.WriteLogAction = AddLogItem;

			InitializeComponent();

			// Init the View of the logging.
			LogItemsControl.ItemsSource = new ObservableCollection<String>();
			_logItemsControl = LogItemsControl;

			DoubleNullableMoneyTexBox = DoubleNullableMoney;
			//DoubleMoneyTexBox = DoubleMoney;

			//DecimalNullableMoneyTexBox = DecimalNullableMoney;
			//DecimalMoneyTexBox = DecimalMoney;
		}

		private void SetAmountRandomValueButton_Click(object sender, RoutedEventArgs e)
		{
			var randomDouble = (new Random().NextDouble() + 1) * 80000d;

			TestingViewModel.DoubleNullableMoney = randomDouble;
			TestingViewModel.DoubleMoney = randomDouble;
			TestingViewModel.DecimalNullableMoney = (Decimal)randomDouble;
			TestingViewModel.DecimalMoney = (Decimal)randomDouble;

		}

		private void ClearLog_Click(object sender, RoutedEventArgs e)
		{
			LogItems.Clear();
		}
	}
}
