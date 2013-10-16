using It3xl.FormattedInput.View.Converter;
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
		/// Cared set before the Group Separator should stay in that position.
		/// </summary>
		[TestMethod]
		[Tag("SetCaretBeforeGroupSeparator")]
		public void SetCaretBeforeGroupSeparator()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "123| 456.74".ToSpecificValue(out beforeInputCaretPosition);
			var input = "123| 456.74".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "123| 456.74".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}
	
	}
}
