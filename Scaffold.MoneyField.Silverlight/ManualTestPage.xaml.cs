﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;

namespace It3xl.Scaffold.MoneyField.Silverlight
{
	public partial class ManualTestPage
	{
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
			var logMessage = logMessageAction();

			var span = DateTime.Now - _lastLogTime;
			if (LogItems.Any()
				&& (TimeSpan.FromMilliseconds(100) < span))
			{
				LogItems.Insert(0, "_");
				Debug.WriteLine("_");
			}
			_lastLogTime = DateTime.Now;

			LogItems.Insert(0, logMessage);
			Debug.WriteLine(logMessage);
		}


		public ManualTestPage()
		{
			NumberToMoneyConverter.ShowExeptionAction = ex => MessageBox.Show(ex.ToString());
			NumberToMoneyConverter.WriteLogAction = AddLogItem;

			InitializeComponent();

			// Init the View of the logging.
			LogItemsControl.ItemsSource = new ObservableCollection<String>();
			_logItemsControl = LogItemsControl;
		}

		private void SetAmountRandomButton_Click(object sender, RoutedEventArgs e)
		{
			var randomDouble = (new Random().NextDouble() + 1) * 80000d;

			TestingViewModel.DoubleNullableMoney = randomDouble;
			TestingViewModel.DoubleMoney = randomDouble;
			TestingViewModel.DecimalNullableMoney = (Decimal)randomDouble;
			TestingViewModel.DecimalMoney = (Decimal)randomDouble;

		}

		private void SetDefaultValueButton_Click(object sender, RoutedEventArgs e)
		{
			TestingViewModel.DoubleNullableMoney = null;
			TestingViewModel.DoubleMoney = 0;
			TestingViewModel.DecimalNullableMoney = null;
			TestingViewModel.DecimalMoney = 0;
		}

		private void ClearLog_Click(object sender, RoutedEventArgs e)
		{
			LogItems.Clear();
		}
	}
}