using System;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class NegativeValuesTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		[TestMethod]
		[Asynchronous]
		[Tag("NegativeValueUnsupported")]
		public void NegativeValueUnsupported()
		{
			_scaffold.ViewModel.DoubleNullableMoney = -123456.78;

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 123456.78);
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "123 456|.78".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
			});

			EnqueueTestComplete();
		}


		// Double behaviours.
		// The Double Min value

		// Decimal behaviours.
		// The Decimal Min value
	
	}
}
