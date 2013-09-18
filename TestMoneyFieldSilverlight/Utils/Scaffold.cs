namespace TestMoneyFieldSilverlight.Utils
{
	using System.Windows.Controls;

	using MoneyField.Silverlight;
	using MoneyField.Silverlight.View;
	using MoneyField.Silverlight.ViewModel;

	using System;

	public class Scaffold
	{
		public static MoneyTextBox TestBoxStatic { get; set; }
		public MoneyTextBox TestBox_ { get; set; }
		public ViewModelForTests ViewModel { get; set; }

		/// <summary>
		/// Initialization of any test method.
		/// </summary>
		public void TestInitialize(Panel testPanel)
		{
			var testPage = new MainPage();

			ViewModel = testPage.DataContext as ViewModelForTests;
			TestBox_ = testPage.TestMoneyTexBox;

			TestBoxStatic = testPage.TestMoneyTexBox;

			testPanel.Children.Add(testPage);
		}
	}
}
