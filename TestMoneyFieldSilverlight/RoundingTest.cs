// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

	[TestClass]
	public class RoundingTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The test of the Double rounding.
		/// </summary>
		[TestMethod]
		public void DoubleRoundingIntegerPart()
		{
			Int32 expectedCaretPosition;

			_scaffold.ViewModel.AmountDouble = 123456789123456789123456789.987654321;

			Assert.IsTrue(_scaffold.TestBox.Text
				== "123 456 789 123 457 000 000 000 000.00"
					.ToSpecificValue(out expectedCaretPosition));
		}

		/// <summary>
		/// Decimal part rounding test.
		/// </summary>
		[TestMethod]
		public void DoubleRoundingForPartialPart()
		{

			String beforeInput;
			Int32 beforeInputCaretPosition;
			String input;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			// 1
			beforeInput = "123 456 789 123 457 000.|00".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 789 123 457 000.2|00".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.0|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			// 2
			beforeInput = "123 456 789 123 457 000.0|0".ToSpecificValue(out beforeInputCaretPosition);
			input = "123 456 789 123 457 000.02|0".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.00|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}



		// TODO.it3xl.com: Проверить, как поведут себя Decimal, Int32, UInt32 для огромных чисел.
	
	
	}
}
