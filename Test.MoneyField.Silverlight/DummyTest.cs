using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class DummyTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// It's the useless test for the demonstration of a parallel testing.
		/// </summary>
		//[Ignore]
		[Description("Useless test to demonstrate the parallel testing.")]
		[TestMethod]
		[Asynchronous]
		public void DummyParallelTestingAsync()
		{

			// HowTo:
			// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
			// Async HowTo:
			// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
			// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing


			Int32 expectedCaretPosition;

			_scaffold.DoubleNullableMoneyTexBox.Text = "0.00".ToSpecificValue(out expectedCaretPosition);
			_scaffold.DoubleNullableMoneyTexBox.SelectionStart = 1;

			//EnqueueConditional(() => true);
			//EnqueueDelay(TimeSpan.FromSeconds(5));

			EnqueueCallback(() => Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == 1, String.Format("First Step: must be 1, but TestBox.SelectionStart == {0}", _scaffold.DoubleNullableMoneyTexBox.SelectionStart)));

			EnqueueCallback(() => _scaffold.DoubleNullableMoneyTexBox.Text = "1.00".ToSpecificValue(out expectedCaretPosition));

			//EnqueueDelay(TimeSpan.FromSeconds(5));

			EnqueueCallback(() =>
			{
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "1.00".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == 0, String.Format("Second Step: must be 1, but TestBox.SelectionStart == {0}", _scaffold.DoubleNullableMoneyTexBox.SelectionStart));
			});

			EnqueueTestComplete();
		}
	
	}
}
