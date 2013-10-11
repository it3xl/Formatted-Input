using System;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{

	[TestClass]
	public class RoundingTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The test of the Double rounding.
		/// </summary>
		[TestMethod]
		public void DoubleRoundingIntegerPart()
		{
			Int32 expectedCaretPosition;

			_scaffold.ViewModel.DoubleNullableMoney = 123456789123456789123456789.987654321;

			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text
				== "123 456 789 123 457 000 000 000 000.00"
					.ToSpecificValue(out expectedCaretPosition));
		}

		/// <summary>
		/// Decimal part rounding test.
		/// </summary>
		[TestMethod]
		public void DoubleRoundingForPartialPart()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			// 1
			var beforeInput = "123 456 789 123 457 000.|00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "123 456 789 123 457 000.2|00".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.0|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 2
			beforeInput = "123 456 789 123 457 000.0|0".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 789 123 457 000.02|0".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.00|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}


		/// <summary>
		/// <see cref="Double.MaxValue"/> rounding test.
		/// </summary>
		[TestMethod]
		[Tag("DoubleMaxValue")]
		public void DoubleMaxValue()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			// Cose the Double.TryParse can't parse the Double.MaxValue, I'll divide the Decimal.MaxValue by 10.
			const double veryLargeDouble = Double.MaxValue / 10;

			var valueBase = veryLargeDouble.ToString("n", TestLanguageTranslator.LanguageCulture);

			// Imitation insertion from a ViewMode by the Conver method.
			var testValue = "|" + valueBase;

			var resultValue = valueBase.Replace(".", "|.");
			//var resultValue = testValue;

			_scaffold.ViewModel.DoubleNullableMoney = veryLargeDouble;
			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == testValue.ToSpecificValue(out expectedCaretPosition));
			// It has the sense just in a parallel testing. Here, it's always equals the Zero.
			//Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == <> );

			var beforeInput = testValue.ToSpecificValue(out beforeInputCaretPosition);
			// Imitation of the TextBox's focus.
			var input = beforeInput;
			inputCaretPositionRef = beforeInputCaretPosition;

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == resultValue.ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}




		// TODO.it3xl.com: Test the rounding for the Decimal.Max.
	
	
	}
}
