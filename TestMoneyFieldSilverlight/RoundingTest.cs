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
			_scaffold.ViewModel.AmountDouble = 123456789123456789123456789.987654321;

			Assert.IsTrue(
				_scaffold.TestBox_.Text == "123 456 789 123 457 000 000 000 000.00".ToSpecificValue()
				);
		}

		/// <summary>
		/// Decimal part rounding test.
		/// </summary>
		[TestMethod]
		public void DoubleRoundingPartialPart()
		{
			String beforeInput;
			Int32 beforeInputCaretPosition;
			String input;
			Int32 inputCaretPositionRef;

			String formatteValueOut;

			// 1
			beforeInput = "123 456 789 123 457 000.00".ToSpecificValue();
			beforeInputCaretPosition = 24;
			input = "123 456 789 123 457 000.200".ToSpecificValue();
			inputCaretPositionRef = 25;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 25);
			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.00".ToSpecificValue());

			// 2
			beforeInput = "123 456 789 123 457 000.00".ToSpecificValue();
			beforeInputCaretPosition = 25;
			input = "123 456 789 123 457 000.020".ToSpecificValue();
			inputCaretPositionRef = 26;

			_scaffold.TestBox_.Converter.FormatAndManageCaret(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 26);
			Assert.IsTrue(formatteValueOut == "123 456 789 123 457 000.00".ToSpecificValue());
		}



		// TODO.it3xl.com: Проверить, как поведут себя Decimal, Int32, UInt32 для огромных чисел.
	
	
	}
}
