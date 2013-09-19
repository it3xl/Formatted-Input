﻿namespace TestMoneyFieldSilverlight.Utils
{
	using System;
	using MoneyField.Silverlight;
	using MoneyField.Silverlight.View;
	using MoneyField.Silverlight.ViewModel;
	using System.Windows.Controls;

	public class Scaffold
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
			TestBox = testPage.TestMoneyTexBox;

			TestBoxStatic = testPage.TestMoneyTexBox;

			testPanel.Children.Add(testPage);
		}

		// TODO.it3xl.com: Test the Decimal, Int32, UInt32
	}
}
