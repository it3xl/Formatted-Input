// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

	/// <summary>
	/// The testing of a integer part of number.
	/// </summary>
	[TestClass]
	public class IntegerDigitsTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

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
			_scaffold.ViewModel.AmountDouble = 50;

			Assert.IsTrue(_scaffold.TestBox_.Text == "50.00".ToSpecificValue());
		}

		/// <summary>
		/// Если при значении 0,00 перед запятой поставить курсор и ввести 1, то курсор должен остаться на том же месте.
		/// </summary>
		[TestMethod]
		public void ReplaceIntegerZeroWithDigitAfterZero()
		{
			var beforeInput = "0.00".ToSpecificValue();
			var beforeInputCaretPosition = 1;
			var input = "01.00".ToSpecificValue();
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
			Assert.IsTrue(formatteValueOut == "1.00".ToSpecificValue());
		}

		/// <summary>
		/// Behavior checking for the input of a single digit symbol at the empty field.
		/// </summary>
		[TestMethod]
		public void FirstDigitSymbolBehavior()
		{
			var beforeInput = String.Empty;
			var beforeInputCaretPosition = 0;
			var input = "4";
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
			Assert.IsTrue(formatteValueOut == "4.00".ToSpecificValue());
		}

		/// <summary>
		/// Adding zero at the start is ignored and dosen't change the caret's position..
		/// </summary>
		[TestMethod]
		public void AddZeroBeforeIntegerPart()
		{
			var beforeInput = "234.00".ToSpecificValue();
			var beforeInputCaretPosition = 0;
			var input = "0234.00".ToSpecificValue();
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == "234.00".ToSpecificValue());
		}

		/// <summary>
		/// If the integer part is the zero, then a input leads to replacing of that zero at the position 0 and 1. 
		/// </summary>
		[TestMethod]
		public void InsertAtPlaceOfZero()
		{
			var beforeInput = "0.03".ToSpecificValue();
			var beforeInputCaretPosition = 0;
			var input = "10.03".ToSpecificValue();
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
			Assert.IsTrue(formatteValueOut == "1.03".ToSpecificValue());


			beforeInput = "0.03".ToSpecificValue();
			beforeInputCaretPosition = 0;
			input = "1 0.03".ToSpecificValue();
			inputCaretPositionRef = 2;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == "1.03".ToSpecificValue());


			beforeInput = "0.03".ToSpecificValue();
			beforeInputCaretPosition = 0;
			input = "1_ 8790.03".ToSpecificValue();
			inputCaretPositionRef = 6;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 5);
			Assert.IsTrue(formatteValueOut == "1 879.03".ToSpecificValue());
		}






		// TODO.it3xl.com: Как-то протестировать основные комбинации с основными разделителями и отступами в числах для Ru, En, Us.


	}



}