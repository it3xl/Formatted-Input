// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

// HowTo:
// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
// Async HowTo:
// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing

using System.Globalization;
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
	public class CaretTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Tests a moving of the caret from the partial's end to the integer's end when the focus is obtained.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		public void MoveCaretFromPartialEndToIntegerEndOnFocus()
		{
			Int32 beforeInputCaretPosition;
			_scaffold.TestBox.Text = "12 334.52|".ToSpecificValue(out beforeInputCaretPosition);
			_scaffold.TestBox.SelectionStart = beforeInputCaretPosition;
			_scaffold.TestBox.Focus();

			//EnqueueConditional(() => true);
			//EnqueueDelay(TimeSpan.FromMilliseconds(500));
			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.TestBox.Text == "12 334|.52".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.TestBox.SelectionStart == expectedCaretPosition);
			}
			);
			EnqueueTestComplete();
		}



	
	}
}
