using System;
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
		public MoneyTextBox TestMoneyTexBox { get; private set; }

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

		private static void AddLogItem(String logMessage)
		{
			if (LogItems.Any()
				&& LogItems.Last() != "_"
				&& (TimeSpan.FromMilliseconds(100) < DateTime.Now - _lastLogTime)
				)
			{
				_lastLogTime = DateTime.Now;

				LogItems.Insert(0, "_");
			}

			LogItems.Insert(0, logMessage);
		}


		public MainPage()
		{

			AnyNumberToMoneyConverter.ShowExeptionAction = ex => MessageBox.Show(ex.ToString());
			AnyNumberToMoneyConverter.WriteLogAction = AddLogItem;


			InitializeComponent();

			// Init the View of the logging.
			LogItemsControl.ItemsSource = new ObservableCollection<String>();
			_logItemsControl = LogItemsControl;

			TestMoneyTexBox = TagetTestMoneyTexBox;

			TagetTestMoneyTexBox.DecimalSeparator = ',';
			TagetTestMoneyTexBox.AlternativeDecimalSeparator = '.';
			TagetTestMoneyTexBox.GroupSeparator = ' ';
		}

		private void SetAmountRandomValueButton_Click(object sender, RoutedEventArgs e)
		{
			var random = (new Random().NextDouble() + 1) * 80000d;
			TestingViewModel.AmountDouble = random;
		}

		private void ClearLog_Click(object sender, RoutedEventArgs e)
		{
			LogItems.Clear();
		}
	}
}
