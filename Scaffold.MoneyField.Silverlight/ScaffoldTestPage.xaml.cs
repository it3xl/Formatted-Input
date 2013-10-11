using System;
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
	public partial class ScaffoldTestPage
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


		private static DateTime _lastLogTime = DateTime.Now;

		private static void WriteToVisualStudioOutput(Func<String> logMessageAction)
		{
			var logMessage = logMessageAction();

			var span = DateTime.Now - _lastLogTime;
			if (TimeSpan.FromMilliseconds(100) < span)
			{
				Debug.WriteLine("__");
			}
			_lastLogTime = DateTime.Now;

			Debug.WriteLine(logMessage);
		}


		public ScaffoldTestPage()
		{
			NumberToMoneyConverter.ShowExeptionAction = ex => MessageBox.Show(ex.ToString());
			NumberToMoneyConverter.WriteLogAction = WriteToVisualStudioOutput;

			InitializeComponent();

			DoubleNullableMoneyTexBox = DoubleNullableMoney;
			DoubleMoneyTexBox = DoubleMoney;

			DecimalNullableMoneyTexBox = DecimalNullableMoney;
			DecimalMoneyTexBox = DecimalMoney;
		}
	}
}
