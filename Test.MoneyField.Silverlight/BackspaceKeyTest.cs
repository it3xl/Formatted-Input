using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class BackspaceKeyTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingZeros()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "8|0 000 009 123.25".ToSpecificValue(out beforeInputCaretPosition);
			var input = "|0 000 009 123.25".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|9 123.25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingSeparator()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "8| 000 009 123.25".ToSpecificValue(out beforeInputCaretPosition);
			var input = "| 000 009 123.25".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|9 123.25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		/// <summary>
		/// Deletion of a digit in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void IntegerDigitBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 11|1 222 333.25".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 1|1 222 333.25".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 01|1 222 333.25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		/// <summary>
		/// Deletion of the Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorInMiddleBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 111 222 |333 444 000.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 111 222|333 444 000.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 011 122| 333 444 000.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of the Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtStartBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 |111 222 333 444 000.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80|111 222 333 444 000.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8| 111 222 333 444 000.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of the Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtEndBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 111 222 333 444 |000.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 111 222 333 444|000.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344| 000.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		/// <summary>
		/// Deletion of a digit after a Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorInMiddleBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 111 222 3|33 444 000.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 111 222 |33 444 000.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 011 122 2|33 444 000.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of a digit after a Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtStartBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 1|11 222 333 444 000.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 |11 222 333 444 000.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 0|11 222 333 444 000.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of a digit after a Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtEndBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 111 222 333 444 0|00.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 111 222 333 444 |00.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 4|00.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "8 111 222 333 444 |000.00".ToSpecificValue(out beforeInputCaretPosition);
			input = "8 111 222 333 444 |00.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "811 122 233 344 4|00.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		/// <summary>
		/// Deletion of the Decimal Separator by the Backspace key.<para/>
		/// It's only moves the cursor to the left.
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "80 111 222.|00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "80 111 222|00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "80 111 222|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Deletion of the Decimal Separator by the Backspace key for the zero value.<para/>
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorBackspacingForZero()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "0.|00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "0|00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		/// <summary>
		/// Partial & Backspacing behaviours.
		/// </summary>
		[TestMethod]
		public void PartialAndBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "45.72|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "45.7|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "45.7|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "45.7|2".ToSpecificValue(out beforeInputCaretPosition);
			input = "45.|2".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "45.|02".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "45.|72".ToSpecificValue(out beforeInputCaretPosition);
			input = "45|72".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "45|.72".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Zero & Backspacing behaviours.
		/// </summary>
		[TestMethod]
		public void ZeroAndBackspacing()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "0|.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "|.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0.00".ToSpecificValue(out beforeInputCaretPosition);
			// An imitation of the focus.
			input = "|0.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|0.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|1.00".ToSpecificValue(out beforeInputCaretPosition);
			// An imitation of the focus.
			input = "|1.00".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|1.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Backspacing before the first partial digit.
		/// </summary>
		[TestMethod]
		public void BackspacingBeforeFirstPartialDigit()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "1.2|3".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1.|3".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1.|03".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

	}
}
