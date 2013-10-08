using System;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class PartialDisabledTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		[TestMethod]
		public void PartialDisabled()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = true;

			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "01|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0".ToSpecificValue(out beforeInputCaretPosition);
			input = "1|0".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "0.|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0".ToSpecificValue(out beforeInputCaretPosition);
			input = ".|0".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "012sdfjkls348sf.|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "12 348|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "1|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "d|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusEnum.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}
		// Test main behaviours for PartialDisabled.
		/* Test cases:
		 * |0
		 * |
		 * |
		 */

		// Test of the ignoring of partial values from ViewModel on a get and on a set. 
		// Test main behaviours for PartialDisabledOnInput.
		// TODO.it3xl.com: PartialDisabledTest:

}
}
