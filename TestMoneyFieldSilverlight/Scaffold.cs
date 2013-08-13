using System.Windows.Controls;
using MoneyField.Silverlight;
using MoneyField.Silverlight.View;
using MoneyField.Silverlight.ViewModel;

namespace TestMoneyFieldSilverlight
{
	public class Scaffold
	{
		public MoneyTextBox TestBox { get; set; }
		public ViewModelForTests ViewModel { get; set; }

		/// <summary>
		/// Any test method common initializing.
		/// </summary>
		public void TestInitialize(Panel testPanel)
		{
			var testPage = new MainPage();

			ViewModel = testPage.DataContext as ViewModelForTests;
			TestBox = testPage.TestMoneyTexBox;

			testPanel.Children.Add(testPage);
		}
	}
}
