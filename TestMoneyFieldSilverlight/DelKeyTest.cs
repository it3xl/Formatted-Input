// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class DelKeyTest : SilverlightTest
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
			var beforeInput = "80 000 009 123.25".ToSpecificValue();
			var beforeInputCaretPosition = 0;
			var input = "0 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "9 123.25".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingSeparator()
		{
			var beforeInput = "8 000 009 123.25".ToSpecificValue();
			var beforeInputCaretPosition = 0;
			var input = " 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "9 123.25".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of a digit in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void IntegerDigitDeletion()
		{
			var beforeInput = "80 111 222 333.25".ToSpecificValue();
			var beforeInputCaretPosition = 4;
			var input = "80 11 222 333.25".ToSpecificValue();
			var inputCaretPositionRef = 4;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "8 011 222 333.25".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of the Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorInMiddleDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 10;
			var input = "80 111 222333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 10;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtStartDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 2;
			var input = "80111 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 2;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "8 011 222 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtEndDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 18;
			var input = "80 111 222 333 444000.00".ToSpecificValue();
			var inputCaretPositionRef = 18;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 19);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 400.00".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of a digit before a Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void DigitBeforeGroupSeparatorInMiddleDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 9;
			var input = "80 111 22 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 9;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 10);
			Assert.IsTrue(formatteValueOut == "8 011 122 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of a digit before a Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void DigitBeforeGroupSeparatorAtStartDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = "8 111 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 2);
			Assert.IsTrue(formatteValueOut == "8 111 222 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of a digit before a Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void DigitBeforeGroupSeparatorAtEndDeletion()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 17;
			var input = "80 111 222 333 44 000.00".ToSpecificValue();
			var inputCaretPositionRef = 17;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 18);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 000.00".ToSpecificValue());


			beforeInput = "8 111 222 333 444 000.00".ToSpecificValue();
			beforeInputCaretPosition = 16;
			input = "8 111 222 333 44 000.00".ToSpecificValue();
			inputCaretPositionRef = 16;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 16);
			Assert.IsTrue(formatteValueOut == "811 122 233 344 000.00".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of the Decimal Separator by the Del key.<para/>
		/// It's only moves the cursor to the left.
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorDeletion()
		{
			var beforeInput = "80 111 222.00".ToSpecificValue();
			var beforeInputCaretPosition = 10;
			var input = "80 111 22200".ToSpecificValue();
			var inputCaretPositionRef = 10;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == "80 111 222.00".ToSpecificValue());
		}


		/// <summary>
		/// Partial & Del key behaviours.
		/// </summary>
		[TestMethod]
		public void PartialAndDelKey()
		{
			String formatteValueOut;

			var beforeInput = "45.72".ToSpecificValue();
			var beforeInputCaretPosition = 2;
			var input = "4572".ToSpecificValue();
			var inputCaretPositionRef = 2;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "45.72".ToSpecificValue());


			beforeInput = "45.72".ToSpecificValue();
			beforeInputCaretPosition = 3;
			input = "45.2".ToSpecificValue();
			inputCaretPositionRef = 3;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 3);
			Assert.IsTrue(formatteValueOut == "45.20".ToSpecificValue());


			beforeInput = "45.72".ToSpecificValue();
			beforeInputCaretPosition = 4;
			input = "45.7".ToSpecificValue();
			inputCaretPositionRef = 4;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "45.70".ToSpecificValue());
		}


	}
}
