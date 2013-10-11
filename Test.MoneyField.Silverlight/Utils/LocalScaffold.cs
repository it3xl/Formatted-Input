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
			var testPage = new ScaffoldTestPage();

			CurrentTestBox = testPage.DoubleNullableMoneyTexBox;

			ViewModel = testPage.DataContext as ViewModelForTests;

			DoubleNullableMoneyTexBox = testPage.DoubleNullableMoneyTexBox;
			DoubleMoneyTexBox = testPage.DoubleMoneyTexBox;

			DecimalNullableMoneyTexBox = testPage.DecimalNullableMoneyTexBox;
			DecimalMoneyTexBox = testPage.DecimalMoneyTexBox;


			DoubleNullableMoneyTexBox.PartialDisabled = false;
			DoubleNullableMoneyTexBox.PartialDisabledOnInput = false;

			DoubleMoneyTexBox.PartialDisabled = false;
			DoubleMoneyTexBox.PartialDisabledOnInput = false;

			DecimalNullableMoneyTexBox.PartialDisabled = false;
			DecimalNullableMoneyTexBox.PartialDisabledOnInput = false;

			DecimalMoneyTexBox.PartialDisabled = false;
			DecimalMoneyTexBox.PartialDisabledOnInput = false;




			testPanel.Children.Add(testPage);
		}
	}
}
