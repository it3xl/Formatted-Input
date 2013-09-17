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

			Assert.IsTrue(_scaffold.TestBox.Text == String.Format("12{1}345{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// The input after DecimalSeparator must lead to the cut the previous first decimal digit.
		/// Not to the movement it to the right.
		/// </summary>
		[TestMethod]
		public void NeedCutSecondPartialDigit()
		{
			var beforeInput = String.Format("23{0}98", _scaffold.TestBox.DecimalSeparator);
			var beforeInputCaretPosition = 3;
			var input = String.Format("23{0}198", _scaffold.TestBox.DecimalSeparator);
			var inputCaretPositionRef = 4;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 4);
			Assert.IsTrue(formatteValueOut == String.Format("23{0}18", _scaffold.TestBox.DecimalSeparator));
		}

		// TODO.it3xl.ru: Partial digits.
		// Typing of the first digit sets the second to the 0. 

	}
}
