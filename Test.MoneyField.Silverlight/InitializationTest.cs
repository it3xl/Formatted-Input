﻿// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

// HowTo:
// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
// Async HowTo:
// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing

using System.Globalization;
using System.Threading;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
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

			Assert.AreEqual(textBox.DecimalSeparator, '.');
			Assert.AreEqual(textBox.DecimalSeparatorAlternative, Char.MinValue);
			Assert.AreEqual(textBox.GroupSeparator, ',');

			textBox.DecimalSeparator = ',';
			textBox.DecimalSeparatorAlternative = '.';
			textBox.GroupSeparator = NumberToMoneyConverter.BreakingSpaceChar;

			Assert.AreEqual(textBox.DecimalSeparator, ',');
			Assert.AreEqual(textBox.DecimalSeparatorAlternative, '.');
			Assert.AreEqual(textBox.GroupSeparator, NumberToMoneyConverter.NonBreakingSpaceChar);

			Thread.CurrentThread.CurrentCulture = lastCulture;
		}

	
	}
}