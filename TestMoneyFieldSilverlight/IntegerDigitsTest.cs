// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
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

			Assert.IsTrue(_scaffold.TestBox.Text == String.Format("50{0}00", _scaffold.TestBox.DecimalSeparator));
		}

		/// <summary>
		/// Если при значении 0,00 перед запятой поставить курсор и ввести 1, то курсор должен остаться на том же месте.
		/// </summary>
		[TestMethod]
		public void ReplaceIntegerZeroWithDigitAfterZero()
		{
			var beforeInput = String.Format("0{0}00", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 1;
			var input = String.Format("01{0}00", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 2;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == String.Format("1{0}00", _scaffold.TestBox.DecimalSeparator));
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

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 1);
			Assert.IsTrue(formatteValueOut == string.Format("4{0}00", _scaffold.TestBox.DecimalSeparator));
		}

		/// <summary>
		/// Adding zero at the start is ignored and dosen't change the caret's position..
		/// </summary>
		[TestMethod]
		public void AddZeroBeforeIntegerPart()
		{
			var beforeInput = String.Format("234{0}00", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 0;
			var input = String.Format("0234{0}00", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 1;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == string.Format("234{0}00", _scaffold.TestBox.DecimalSeparator));
		}






		// TODO.it3xl.com: Как-то протестировать основные комбинации с основными разделителями и отступами в числах для Ru, En, Us.


	}



}