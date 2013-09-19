// ReSharper disable ConvertToConstant.Local
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

			Assert.IsTrue(_scaffold.TestBox.Text == "12 345.00".ToSpecificValue());
		}

		/// <summary>
		/// The input after DecimalSeparator must lead to the cut the previous first decimal digit.
		/// Not to the movement it to the right.
		/// </summary>
		[TestMethod]
		public void NeedCutSecondPartialDigit()
		{
			String formatteValueOut;

			var beforeInput = "23.98".ToSpecificValue();
			var beforeInputCaretPosition = 3;
			var input = "23.198".ToSpecificValue();
			var inputCaretPositionRef = 4;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == "23.18".ToSpecificValue());
		}

	}
}
