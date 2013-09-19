﻿// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	/// <summary>
	/// The testing of a integer part of number.
	/// </summary>
	[TestClass]
	public class PartialDigitsTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Decimal part cutting test.
		/// </summary>
		[TestMethod]
		public void PartialPartCutting()
		{
			_scaffold.ViewModel.AmountDouble = 12345.000432;

			Int32 expectedCaretPosition;
			Assert.IsTrue(_scaffold.TestBox.Text == "|12 345.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(_scaffold.TestBox.SelectionStart == expectedCaretPosition);
		}

		/// <summary>
		/// The input after DecimalSeparator must lead to the cut the previous first decimal digit.
		/// Not to the movement it to the right.
		/// </summary>
		[TestMethod]
		public void NeedCutSecondPartialDigit()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "23.|98".ToSpecificValue(out beforeInputCaretPosition);
			var input = "23.1|98".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "23.1|8".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

	}
}
