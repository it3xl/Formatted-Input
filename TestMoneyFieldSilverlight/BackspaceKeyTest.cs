// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class BackspaceKeyTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

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
			String formatteValueOut;

			var beforeInput = "80 000 009 123.25".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = "0 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "9 123.25".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingSeparator()
		{
			String formatteValueOut;

			var beforeInput = "8 000 009 123.25".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = " 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "9 123.25".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of a digit in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void IntegerDigitBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333.25".ToSpecificValue();
			var beforeInputCaretPosition = 5;
			var input = "80 11 222 333.25".ToSpecificValue();
			var inputCaretPositionRef = 4;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "8 011 222 333.25".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of the Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorInMiddleBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 11;
			var input = "80 111 222333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 10;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == "8 011 122 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtStartBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 3;
			var input = "80111 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 2;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "8 111 222 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtEndBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 19;
			var input = "80 111 222 333 444000.00".ToSpecificValue();
			var inputCaretPositionRef = 18;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 17);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 000.00".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of a digit after a Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorInMiddleBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 12;
			var input = "80 111 222 33 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 11;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of a digit after a Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtStartBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 4;
			var input = "80 11 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 3;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "8 011 222 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of a digit after a Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtEndBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 20;
			var input = "80 111 222 333 444 00.00".ToSpecificValue();
			var inputCaretPositionRef = 19;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 19);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 400.00".ToSpecificValue());


			beforeInput = "8 111 222 333 444 000.00".ToSpecificValue();
			beforeInputCaretPosition = 19;
			input = "8 111 222 333 444 00.00".ToSpecificValue();
			inputCaretPositionRef = 18;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 17);
			Assert.IsTrue(formatteValueOut == "811 122 233 344 400.00".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of the Decimal Separator by the Backspace key.<para/>
		/// It's only moves the cursor to the left.
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "80 111 222.00".ToSpecificValue();
			var beforeInputCaretPosition = 11;
			var input = "80 111 22200".ToSpecificValue();
			var inputCaretPositionRef = 10;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 10);
			Assert.IsTrue(formatteValueOut == "80 111 222.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Decimal Separator by the Backspace key for the zero value.<para/>
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorBackspacingForZero()
		{
			String formatteValueOut;

			var beforeInput = "0.00".ToSpecificValue();
			var beforeInputCaretPosition = 2;
			var input = "000".ToSpecificValue();
			var inputCaretPositionRef = 1;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "0.00".ToSpecificValue());
		}



		/// <summary>
		/// Partial & Backspacing behaviours.
		/// </summary>
		[TestMethod]
		public void PartialAndBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "45.72".ToSpecificValue();
			var beforeInputCaretPosition = 5;
			var input = "45.7".ToSpecificValue();
			var inputCaretPositionRef = 4;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "45.70".ToSpecificValue());


			beforeInput = "45.72".ToSpecificValue();
			beforeInputCaretPosition = 4;
			input = "45.2".ToSpecificValue();
			inputCaretPositionRef = 3;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "45.20".ToSpecificValue());


			beforeInput = "45.72".ToSpecificValue();
			beforeInputCaretPosition = 3;
			input = "4572".ToSpecificValue();
			inputCaretPositionRef = 2;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == "45.72".ToSpecificValue());
		}

		/// <summary>
		/// Partial & Backspacing behaviours.
		/// </summary>
		[TestMethod]
		public void ZeroAndBackspacing()
		{
			String formatteValueOut;

			var beforeInput = "0.00".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = ".00".ToSpecificValue();
			var inputCaretPositionRef = 0;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "0.00".ToSpecificValue());


			beforeInput = "0.00".ToSpecificValue();
			beforeInputCaretPosition = 0;
			input = "0.00".ToSpecificValue();
			inputCaretPositionRef = 0;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "0.00".ToSpecificValue());
		}

	}
}
