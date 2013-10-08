using System.Globalization;
using System.Threading;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Scaffold.MoneyField.Silverlight;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class InitializationTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Tests start states and a changing of the states.
		/// </summary>
		[TestMethod]
		public void StartStatesAndStatesChanging()
		{
			var textBox = new MoneyTextBox();

			var lastCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			Assert.IsTrue(textBox.DecimalSeparatorChar == ".");
			Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == Char.MinValue.ToString(CultureInfo.InvariantCulture));
			Assert.IsTrue(textBox.GroupSeparatorChar == ",");

			textBox.DecimalSeparatorChar = ",";
			textBox.DecimalSeparatorAlternativeChar = ".";
			textBox.GroupSeparatorChar = NumberToMoneyConverter.BreakingSpaceChar.ToString(CultureInfo.InvariantCulture);

			Assert.IsTrue(textBox.DecimalSeparatorChar == ",");
			Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == ".");
			Assert.IsTrue(textBox.GroupSeparatorChar == NumberToMoneyConverter.NonBreakingSpaceChar.ToString(CultureInfo.InvariantCulture));

			Thread.CurrentThread.CurrentCulture = lastCulture;
		}

		/// <summary>
		/// Tests the correct work of the <see cref="NumberToMoneyConverter.TextBeforeChanging"/> on the start.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		public void TextBeforeChangingInitedOnStartAsync()
		{
			var viewModel = new ViewModelForTests
				{
					DoubleNullableMoney = 0
				};
			var testPage = new MainPage{DataContext = viewModel};
			TestPanel.Children.Add(testPage);

			EnqueueCallback(() =>
				{
					Int32 expectedCaretPosition;

					Assert.AreEqual(
						"|0.00".ToSpecificValue(out expectedCaretPosition),
						testPage.DoubleNullableMoneyTexBox.Converter.TextBeforeChanging
					);
					Assert.AreEqual(expectedCaretPosition, testPage.DoubleNullableMoneyTexBox.SelectionStart);
				});

			EnqueueTestComplete();
		}

	
	}
}
