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
		[Tag("MoveCaretFromPartialEndToIntegerEndOnFocusAsync")]
		public void MoveCaretFromPartialEndToIntegerEndOnFocusAsync()
		{
			Int32 beforeInputCaretPosition;
			_scaffold.DoubleNullableMoneyTexBox.Text = "12 334.52|".ToSpecificValue(out beforeInputCaretPosition);
			_scaffold.DoubleNullableMoneyTexBox.SelectionStart = beforeInputCaretPosition;

			this.PrepareFocusFromDebugger(_scaffold.DoubleNullableMoneyTexBox);

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 334|.52".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
			}
			);

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests seting from a ViewModel for the PartialDisabled = true;
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("SetCaretBeforeGroupSeparatorAsync")]
		public void SetCaretBeforeGroupSeparatorAsync()
		{
			//STUB.it3xl.com: restor the test.
			EnqueueTestComplete();
			return;




			_scaffold.ViewModel.DoubleNullableMoney = 123456.74;
			_scaffold.DoubleNullableMoneyTexBox.SelectionStart = 3;

			this.PrepareFocusFromDebugger(_scaffold.DoubleNullableMoneyTexBox);
			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "123| 456.74".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
			});

			EnqueueTestComplete();
		}
	
	}
}
