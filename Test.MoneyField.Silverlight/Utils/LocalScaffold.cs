using It3xl.FormattedInput.View;
using It3xl.Scaffold.MoneyField.Silverlight;
using System.Windows.Controls;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{

	public class LocalScaffold
	{
		public static MoneyTextBox TestBoxStatic { get; set; }
		public MoneyTextBox TestBox { get; set; }
		public ViewModelForTests ViewModel { get; set; }

		/// <summary>
		/// Initialization of any test method.
		/// </summary>
		public void TestInitialize(Panel testPanel)
		{
			var testPage = new MainPage();

			ViewModel = testPage.DataContext as ViewModelForTests;
			TestBox = testPage.DoubleNullableMoneyTexBox;

			TestBoxStatic = testPage.DoubleNullableMoneyTexBox;

			testPanel.Children.Add(testPage);
		}
	}
}
