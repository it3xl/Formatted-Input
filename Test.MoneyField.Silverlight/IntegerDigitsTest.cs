using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	/// <summary>
	/// The testing of a integer part of number.
	/// </summary>
	[TestClass]
	public class IntegerDigitsTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Just a integer input needs a decimal trailing part.
		/// </summary>
		[TestMethod]
		public void AddTwoZeroForInteger()
		{
			_scaffold.ViewModel.DoubleNullableMoney = 50;

			Int32 expectedCaretPosition;
			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "|50.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
		}

		/// <summary>
		/// Если при значении 0,00 перед запятой поставить курсор и ввести 1, то курсор должен остаться на том же месте.
		/// </summary>
		[TestMethod]
		public void ReplaceIntegerZeroWithDigitAfterZero()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "0|.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "01|.00".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Behavior checking for the input of a single digit symbol at the empty field.
		/// </summary>
		[TestMethod]
		public void FirstDigitSymbolBehavior()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "4|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "4|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Adding zero at the start is ignored and dosen't change the caret's position..
		/// </summary>
		[TestMethod]
		public void AddZeroBeforeIntegerPart()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|234.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "0|234.00".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "|234.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// If the integer part is the zero, then a input leads to replacing of that zero at the position 0 and 1. 
		/// </summary>
		[TestMethod]
		public void InsertAtPlaceOfZero()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|0.03".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1|0.03".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1|.03".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "|0.03".ToSpecificValue(out beforeInputCaretPosition);
			input = "1 |0.03".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1|.03".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "|0.03".ToSpecificValue(out beforeInputCaretPosition);
			input = "1_ 879|0.03".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1 879|.03".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

	}

}