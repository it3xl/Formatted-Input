// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
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
			_scaffold.TestBox.Focus();
			_scaffold.TestBox.Text = _scaffold.TestBox.DecimalSeparator.ToString(CultureInfo.InvariantCulture);
			_scaffold.TestBox.SelectionStart = 1;

			//EnqueueConditional(() => true);
			//EnqueueDelay(TimeSpan.FromMilliseconds(500));
			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.TestBox.Text == String.Format("0{0}00", _scaffold.TestBox.DecimalSeparator));
					Assert.IsTrue(_scaffold.TestBox.SelectionStart == 2);
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


			beforeInput = String.Format("1{0}25", _scaffold.TestBox.DecimalSeparator);
			beforeInputCaretPosition = 1;
			input = String.Format("1{0}{0}25", _scaffold.TestBox.DecimalSeparator);
			inputCaretPositionRef = 2;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == String.Format(beforeInput, _scaffold.TestBox.DecimalSeparator));


			beforeInput = String.Format("123{1}456{1}789" + "{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 11;
			input = String.Format("123{1}456{1}789{0}{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 12;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 12);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// Input of the separator after the existed separator.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterSeparator()
		{
			var beforeInput = String.Format("1{0}25", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 1;
			var input = String.Format("1{0}{0}25", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 3;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == String.Format(beforeInput, _scaffold.TestBox.DecimalSeparator));
		}

		/// <summary>
		/// Input of the separator after the decimal digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterDecimal()
		{
			var beforeInput = String.Format("1{0}25", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 3;
			var input = String.Format("1{0}2{0}5", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 4;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == String.Format(beforeInput, _scaffold.TestBox.DecimalSeparator));
		}

		/// <summary>
		/// Input of the separator after the hundredth digit.
		/// It's ignored and don't change the caret position.
		/// </summary>
		[TestMethod]
		public void InputSeparatorAfterHandredth()
		{
			var beforeInput = String.Format("1{0}25", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 4;
			var input = String.Format("1{0}25{0}", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 5;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == String.Format(beforeInput, _scaffold.TestBox.DecimalSeparator));
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
			beforeInput = String.Format("1" + "23{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 1;
			input = String.Format("1{0}23{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 2;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == String.Format("1{0}23", _scaffold.TestBox.DecimalSeparator));

			// 2
			beforeInput = String.Format("12" + "3{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 2;
			input = String.Format("12{0}3{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 3;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == String.Format("12{0}34", _scaffold.TestBox.DecimalSeparator));

			// 3
			beforeInput = String.Format("123" + "{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 3;
			input = String.Format("123{0}{1}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 4;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == String.Format("123{0}45", _scaffold.TestBox.DecimalSeparator));

			// 4
			beforeInput = String.Format("123{1}" + "456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 4;
			input = String.Format("123{1}{0}456{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 5;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == String.Format("123{0}45", _scaffold.TestBox.DecimalSeparator));

			// 5
			beforeInput = String.Format("123{1}4" + "56{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 5;
			input = String.Format("123{1}4{0}56{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 6;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 6);
			Assert.IsTrue(formatteValueOut == String.Format("1{1}234{0}56", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 6
			beforeInput = String.Format("123{1}45" + "6{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 6;
			input = String.Format("123{1}45{0}6{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 7;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 7);
			Assert.IsTrue(formatteValueOut == String.Format("12{1}345{0}67", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 7
			beforeInput = String.Format("123{1}456" + "{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 7;
			input = String.Format("123{1}456{0}{1}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 8;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 8);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{0}78", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 8
			beforeInput = String.Format("123{1}456{1}" + "789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 8;
			input = String.Format("123{1}456{1}{0}789{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 9;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 8);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{0}78", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 9
			beforeInput = String.Format("123{1}456{1}7" + "89{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 9;
			input = String.Format("123{1}456{1}7{0}89{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 10;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 10);
			Assert.IsTrue(formatteValueOut == String.Format("1{1}234{1}567{0}89", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 10
			beforeInput = String.Format("123{1}456{1}78" + "9{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 10;
			input = String.Format("123{1}456{1}78{0}9{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 11;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == String.Format("12{1}345{1}678{0}92", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}


		/// <summary>
		/// Input of the separator before the number part.
		/// It moves digits after separator to the fractional part and sets the caret position after the separator.
		/// Before the separator will be zero.
		/// </summary>
		[TestMethod]
		public void InputSeparatorBeforNumber()
		{
			var beforeInput = String.Format("1{0}25", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 0;
			var input = String.Format("{0}1{0}25", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == String.Format("0{0}12", _scaffold.TestBox.DecimalSeparator));
		}


		// TODO.it3xl.ru: Decimal separator.
		// 0000123 inserting with caret at the 3. Decimal separator needing.


	}



}