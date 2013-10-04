using It3xl.FormattedInput.View;
using It3xl.Scaffold.MoneyField.Silverlight;
using System.Windows.Controls;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{

	public class LocalScaffold
	{
		public static MoneyTextBox CurrentTestBox { get; set; }

		public ViewModelForTests ViewModel { get; set; }

		public MoneyTextBox DoubleNullableMoneyTexBox { get; set; }
		public MoneyTextBox DoubleMoneyTexBox { get; set; }
		public MoneyTextBox DecimalNullableMoneyTexBox { get; set; }
		public MoneyTextBox DecimalMoneyTexBox { get; set; }

		/// <summary>
		/// Initialization of any test method.
		/// </summary>
		public void TestInitialize(Panel testPanel)
		{
			var testPage = new MainPage();

			CurrentTestBox = testPage.DoubleNullableMoneyTexBox;

			ViewModel = testPage.DataContext as ViewModelForTests;

			DoubleNullableMoneyTexBox = testPage.DoubleNullableMoneyTexBox;
			DoubleMoneyTexBox = testPage.DoubleMoneyTexBox;

			DecimalNullableMoneyTexBox = testPage.DecimalNullableMoneyTexBox;
			DecimalMoneyTexBox = testPage.DecimalMoneyTexBox;

			testPanel.Children.Add(testPage);
		}
	}
}
