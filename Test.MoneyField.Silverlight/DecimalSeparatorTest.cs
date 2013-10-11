using System.Threading;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class DecimalSeparatorTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The decimal separator added first behavior.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("SeparatorAddedFirstOnlyAsync")]
		public void SeparatorAddedFirstOnlyAsync()
		{
			_scaffold.DoubleNullableMoneyTexBox.Focus();
			_scaffold.DoubleNullableMoneyTexBox.Text = _scaffold.DoubleNullableMoneyTexBox.DecimalSeparatorChar.ToString(CultureInfo.InvariantCulture);
			_scaffold.DoubleNullableMoneyTexBox.SelectionStart = 1;

			//EnqueueConditional(() => true);
			//EnqueueDelay(TimeSpan.FromMilliseconds(500));
			EnqueueCallback(() =>
				{
					Int32 expectedCaretPosition;
					
					Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "0.|00".ToSpecificValue(out expectedCaretPosition));
					Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
				}
			);
			EnqueueTestComplete();
		}

		/// <summary>
		/// Input of the separator before a separator must delete the last separator.
		/// The caret position must be after first separator at the result string.
		/// </summary>
		[TestMethod]
		public void InputSeparatorBeforSeparator()
		{

			String beforeInput;
			Int32 beforeInputCaretPosition;
			String input;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;


			beforeInput = "1|.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "1.|.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1.|25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "123 456 789|.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 789.|.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 789.|25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Input of the separator after the existed separator.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterSeparator()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "1.|25".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1..|25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1.|25".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Input of the separator after the decimal digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterDecimal()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "1.2|5".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1.2.|5".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1.2|5".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Input of the separator after the hundredth digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterHandredth()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "1.25|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1.25.|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1.25|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Input of the separator among a number.
		/// It moves digits after separator to the fractional part and sets the caret position after the separator.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAmongNumberPart()
		{
			String formatteValueOut;

			String beforeInput;
			Int32 beforeInputCaretPosition;
			String input;
			Int32 inputCaretPositionRef;
			Int32 expectedCaretPosition;

			// 1
			beforeInput = "1|23 456 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "1.|23 456 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1.|23".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 2
			beforeInput = "12|3 456 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "12.|3 456 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12.|34".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 3
			beforeInput = "123| 456 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123.| 456 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123.|45".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 4
			beforeInput = "123 |456 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 .|456 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123.|45".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 5
			beforeInput = "123 4|56 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 4.|56 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1 234.|56".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 6
			beforeInput = "123 45|6 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 45.|6 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.|67".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 7
			beforeInput = "123 456| 789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456.| 789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456.|78".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 8
			beforeInput = "123 456 |789.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 .|789.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456.|78".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 9
			beforeInput = "123 456 7|89.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 7.|89.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1 234 567.|89".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 10
			beforeInput = "123 456 78|9.25".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 78.|9.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345 678.|92".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}


		/// <summary>
		/// Input of the separator before the number part.
		/// It moves digits after separator to the fractional part and sets the caret position after the separator.
		/// Before the separator will be zero.
		/// </summary>
		[TestMethod]
		public void InputSeparatorBeforNumber()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|1.25".ToSpecificValue(out beforeInputCaretPosition);
			var input = ".|1.25".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(input, beforeInput, beforeInputCaretPosition, FocusState.Gotten, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "0.|12".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}


		/// <summary>
		/// Some initiating settings for the <see cref="NumberToMoneyConverter.DecimalSeparator"/>.
		/// </summary>
		[TestMethod]
		public void InitiatingSettings()
		{
			var converter = new NumberToMoneyConverter(new MoneyTextBox())
				{
					DecimalSeparator = Char.MinValue,
				};

			Assert.IsTrue(converter.DecimalSeparator.ToString() == Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
		}


	}



}