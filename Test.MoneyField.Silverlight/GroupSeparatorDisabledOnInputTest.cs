using System.Globalization;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class GroupSeparatorDisabledOnInputTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// GroupSeparatorDisabled main behaviours.
		/// </summary>
		[TestMethod]
		[Tag("GroupSeparatorDisabledMainBehaviours")]
		public void GroupSeparatorDisabledMainBehaviours()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			// !!!
			_scaffold.DoubleNullableMoneyTexBox.Converter
				.GroupSeparatorDisabledOnInput = true;

			var beforeInput = "123 456 789|.72".ToSpecificValue(out beforeInputCaretPosition);
			var input = "123 45|6 789.72".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "12345|6789.72".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "123 456 789|.72".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456| 789.72".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "123456|789.72".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "123 456 789|.72".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 |789.72".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "123456|789.72".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "123 456 789|.72".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 7|89.72".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1234567|89.72".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}
	
	}
}
