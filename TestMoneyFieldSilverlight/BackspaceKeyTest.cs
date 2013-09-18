// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

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
			var beforeInput = "80 000 009 123.25".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = "0 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
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
			var beforeInputCaretPosition = 1;
			var input = " 000 009 123.25".ToSpecificValue();
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
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
		public void IntegerDigitBackspacing()
		{
			var beforeInput = "80 111 222 333.25".ToSpecificValue();
			var beforeInputCaretPosition = 5;
			var input = "80 11 222 333.25".ToSpecificValue();
			var inputCaretPositionRef = 4;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
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
		public void GroupSeparatorInMiddleBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 11;
			var input = "80 111 222333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 10;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == "8 011 122 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtStartBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 3;
			var input = "80111 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 2;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "8 111 222 333 444 000.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorAtEndBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 19;
			var input = "80 111 222 333 444000.00".ToSpecificValue();
			var inputCaretPositionRef = 18;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 17);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 000.00".ToSpecificValue());
		}



		/// <summary>
		/// Deletion of a digit after a Group Separator in the middle part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorInMiddleBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 12;
			var input = "80 111 222 33 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 11;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
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
		/// Deletion of a digit after a Group Separator at the beginning part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtStartBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 4;
			var input = "80 11 222 333 444 000.00".ToSpecificValue();
			var inputCaretPositionRef = 3;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
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
		/// Deletion of a digit after a Group Separator at the end part of a number.
		/// </summary>
		[TestMethod]
		public void DigitAfterGroupSeparatorAtEndBackspacing()
		{
			var beforeInput = "80 111 222 333 444 000.00".ToSpecificValue();
			var beforeInputCaretPosition = 20;
			var input = "80 111 222 333 444 00.00".ToSpecificValue();
			var inputCaretPositionRef = 19;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 19);
			Assert.IsTrue(formatteValueOut == "8 011 122 233 344 400.00".ToSpecificValue());


			beforeInput = "8 111 222 333 444 000.00".ToSpecificValue();
			beforeInputCaretPosition = 19;
			input = "8 111 222 333 444 00.00".ToSpecificValue();
			inputCaretPositionRef = 18;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

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
			var beforeInput = "80 111 222.00".ToSpecificValue();
			var beforeInputCaretPosition = 11;
			var input = "80 111 22200".ToSpecificValue();
			var inputCaretPositionRef = 10;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 10);
			Assert.IsTrue(formatteValueOut == "80 111 222.00".ToSpecificValue());
		}

		/// <summary>
		/// Deletion of the Decimal Separator by the Backspace key for the zero value.<para/>
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorBackspacingForZero()
		{
			var beforeInput = "0.00".ToSpecificValue();
			var beforeInputCaretPosition = 2;
			var input = "000".ToSpecificValue();
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "0.00".ToSpecificValue());
		}




		// TODO.it3xl.com: The Backspace key.

		// Backspace в дробной части.
		// Должен отрабатывать, как стрелка влево с обнулением чисел.
		// 0,00 - курсор должен оставаться в позиции 1 (т.е. Backspace должен отработать, как стрелка влево)
		// 0,00 - если курсор в первой позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.
		// 0,00 - если курсор в 0 позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.

		// Протестировать поведение GroupSeparator в разных случаях.
		// Протестировать поведение DecimalSeparator в разных случаях. Перенести сюда соответствующие тесты из DecimalSeparatorTest.

	}
}
