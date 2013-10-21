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
		[Tag("StartStatesAndStatesChanging")]
		public void StartStatesAndStatesChanging()
		{
			var textBox = new MoneyTextBox();

			Assert.IsTrue(textBox.DecimalSeparatorChar == ".");
			Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == String.Empty);
			Assert.IsTrue(textBox.GroupSeparatorChar == String.Empty);

			textBox.DecimalSeparatorChar = ",";
			textBox.DecimalSeparatorAlternativeChar = ".";
			textBox.GroupSeparatorChar = NumberToMoneyConverter.BreakingSpaceChar.ToString(CultureInfo.InvariantCulture);

			Assert.IsTrue(textBox.DecimalSeparatorChar == ",");
			Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == ".");
			Assert.IsTrue(textBox.GroupSeparatorChar == NumberToMoneyConverter.NonBreakingSpaceChar.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Tests a setting of the separators to a digit.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("SetSeparatorsToDigit")]
		public void SetSeparatorsToDigit()
		{
			var textBox = _scaffold.DoubleNullableMoneyTexBox;

			EnqueueCallback(() =>
				{
					textBox.DecimalSeparatorChar = "1";
					textBox.DecimalSeparatorAlternativeChar = "2";
					textBox.GroupSeparatorChar = "3";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == "1");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == "2");
					Assert.IsTrue(textBox.GroupSeparatorChar == "3");

					textBox.Text = "*";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == ".");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == String.Empty);
					Assert.IsTrue(textBox.GroupSeparatorChar == String.Empty);
				});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests a setting of the separators to a same value.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("SetAllSeparatorsTheSame")]
		public void SetAllSeparatorsTheSame()
		{
			var textBox = _scaffold.DoubleNullableMoneyTexBox;

			EnqueueCallback(() =>
				{
					textBox.DecimalSeparatorChar = "^";
					textBox.DecimalSeparatorAlternativeChar = "^";
					textBox.GroupSeparatorChar = "^";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == "^");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == "^");
					Assert.IsTrue(textBox.GroupSeparatorChar == "^");

					textBox.Text = "*";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == "^");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == String.Empty);
					Assert.IsTrue(textBox.GroupSeparatorChar == String.Empty);
				});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests a setting of the Group and Decimal Alternative separators to a same value.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("SetGroupAndAlternativeSeparatorsTheSame")]
		public void SetGroupAndAlternativeSeparatorsTheSame()
		{
			var textBox = _scaffold.DoubleNullableMoneyTexBox;

			EnqueueCallback(() =>
				{
					textBox.DecimalSeparatorChar = "!";
					textBox.DecimalSeparatorAlternativeChar = "^";
					textBox.GroupSeparatorChar = "^";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == "!");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == "^");
					Assert.IsTrue(textBox.GroupSeparatorChar == "^");

					textBox.Text = "*";
				});

			EnqueueCallback(() =>
				{
					Assert.IsTrue(textBox.DecimalSeparatorChar == "!");
					Assert.IsTrue(textBox.DecimalSeparatorAlternativeChar == String.Empty);
					Assert.IsTrue(textBox.GroupSeparatorChar == "^");
				});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests the correct work of the <see cref="NumberToMoneyConverter.TextBeforeChangingNotNull"/> on the start.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("ValueInitedBeforeStartAsync")]
		public void ValueInitedBeforeStartAsync()
		{
			var viewModel = new ViewModelForTests
				{
					DoubleNullableMoney = 0
				};
			var testPage = new ScaffoldTestPage{DataContext = viewModel};
			TestPanel.Children.Add(testPage);

			EnqueueCallback(() =>
				{
					Int32 expectedCaretPosition;

					Assert.IsTrue(
						"0|.00".ToSpecificValue(out expectedCaretPosition)
						== testPage.DoubleNullableMoneyTexBox.Converter.TextBeforeChangingNotNull
					);
					Assert.IsTrue(expectedCaretPosition == testPage.DoubleNullableMoneyTexBox.SelectionStart);
				});

			EnqueueTestComplete();
		}

	
	}
}
