// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

	[TestClass]
	public class DecimalSeparatorTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

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
		public void SeparatorAddedFirstOnly()
		{
			_scaffold.TestBox_.Focus();
			_scaffold.TestBox_.Text = _scaffold.TestBox_.DecimalSeparator.ToString(CultureInfo.InvariantCulture);
			_scaffold.TestBox_.SelectionStart = 1;

			//EnqueueConditional(() => true);
			//EnqueueDelay(TimeSpan.FromMilliseconds(500));
			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.TestBox_.Text == "0.00".ToSpecificValue());
					Assert.IsTrue(_scaffold.TestBox_.SelectionStart == 2);
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


			beforeInput = "1.25".ToSpecificValue();
			beforeInputCaretPosition = 1;
			input = "1..25".ToSpecificValue();
			inputCaretPositionRef = 2;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == beforeInput);


			beforeInput = "123 456 789" + ".25".ToSpecificValue();
			beforeInputCaretPosition = 11;
			input = "123 456 789..25".ToSpecificValue();
			inputCaretPositionRef = 12;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 12);
			Assert.IsTrue(formatteValueOut == "123 456 789.25".ToSpecificValue());
		}

		/// <summary>
		/// Input of the separator after the existed separator.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterSeparator()
		{
			var beforeInput = "1.25".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = "1..25".ToSpecificValue();
			var inputCaretPositionRef = 3;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == beforeInput);
		}

		/// <summary>
		/// Input of the separator after the decimal digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterDecimal()
		{
			var beforeInput = "1.25".ToSpecificValue();
			var beforeInputCaretPosition = 3;
			var input = "1.2.5".ToSpecificValue();
			var inputCaretPositionRef = 4;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == beforeInput);
		}

		/// <summary>
		/// Input of the separator after the hundredth digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterHandredth()
		{
			var beforeInput = "1.25".ToSpecificValue();
			var beforeInputCaretPosition = 4;
			var input = "1.25.".ToSpecificValue();
			var inputCaretPositionRef = 5;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == beforeInput);
		}

		/// <summary>
		/// Input of the separator among a number.
		/// It moves digits after separator to the fractional part and sets the caret position after the separator.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAmongNumberPart()
		{
			String beforeInput;
			Int32 beforeInputCaretPosition;
			String input;
			Int32 inputCaretPositionRef;

			String formatteValueOut;

			// 1
			beforeInput = "1" + "23 456 789.25".ToSpecificValue();
			beforeInputCaretPosition = 1;
			input = "1.23 456 789.25".ToSpecificValue();
			inputCaretPositionRef = 2;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == "1.23".ToSpecificValue());

			// 2
			beforeInput = "12" + "3 456 789.25".ToSpecificValue();
			beforeInputCaretPosition = 2;
			input = "12.3 456 789.25".ToSpecificValue();
			inputCaretPositionRef = 3;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "12.34".ToSpecificValue());

			// 3
			beforeInput = "123" + " 456 789.25".ToSpecificValue();
			beforeInputCaretPosition = 3;
			input = "123. 456 789.25".ToSpecificValue();
			inputCaretPositionRef = 4;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "123.45".ToSpecificValue());

			// 4
			beforeInput = "123 " + "456 789.25".ToSpecificValue();
			beforeInputCaretPosition = 4;
			input = "123 .456 789.25".ToSpecificValue();
			inputCaretPositionRef = 5;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "123.45".ToSpecificValue());

			// 5
			beforeInput = "123 4" + "56 789.25".ToSpecificValue();
			beforeInputCaretPosition = 5;
			input = "123 4.56 789.25".ToSpecificValue();
			inputCaretPositionRef = 6;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 6);
			Assert.IsTrue(formatteValueOut == "1 234.56".ToSpecificValue());

			// 6
			beforeInput = "123 45" + "6 789.25".ToSpecificValue();
			beforeInputCaretPosition = 6;
			input = "123 45.6 789.25".ToSpecificValue();
			inputCaretPositionRef = 7;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 7);
			Assert.IsTrue(formatteValueOut == "12 345.67".ToSpecificValue());

			// 7
			beforeInput = "123 456" + " 789.25".ToSpecificValue();
			beforeInputCaretPosition = 7;
			input = "123 456. 789.25".ToSpecificValue();
			inputCaretPositionRef = 8;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 8);
			Assert.IsTrue(formatteValueOut == "123 456.78".ToSpecificValue());

			// 8
			beforeInput = "123 456 " + "789.25".ToSpecificValue();
			beforeInputCaretPosition = 8;
			input = "123 456 .789.25".ToSpecificValue();
			inputCaretPositionRef = 9;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 8);
			Assert.IsTrue(formatteValueOut == "123 456.78".ToSpecificValue());

			// 9
			beforeInput = "123 456 7" + "89.25".ToSpecificValue();
			beforeInputCaretPosition = 9;
			input = "123 456 7.89.25".ToSpecificValue();
			inputCaretPositionRef = 10;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 10);
			Assert.IsTrue(formatteValueOut == "1 234 567.89".ToSpecificValue());

			// 10
			beforeInput = "123 456 78" + "9.25".ToSpecificValue();
			beforeInputCaretPosition = 10;
			input = "123 456 78.9.25".ToSpecificValue();
			inputCaretPositionRef = 11;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == "12 345 678.92".ToSpecificValue());
		}


		/// <summary>
		/// Input of the separator before the number part.
		/// It moves digits after separator to the fractional part and sets the caret position after the separator.
		/// Before the separator will be zero.
		/// </summary>
		[TestMethod]
		public void InputSeparatorBeforNumber()
		{
			var beforeInput = "1.25".ToSpecificValue();
			var beforeInputCaretPosition = 0;
			var input = ".1.25".ToSpecificValue();
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == "0.12".ToSpecificValue());
		}


		// TODO.it3xl.com: Decimal separator.
		// 0000123 inserting with caret at the 3. Decimal separator needing.


	}



}