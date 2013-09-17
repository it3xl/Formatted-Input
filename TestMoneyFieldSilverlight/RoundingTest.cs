// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
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
				_scaffold.TestBox.Text == String.Format(
					"123{1}456{1}789{1}123{1}457{1}000{1}000{1}000{1}000{0}00",
					_scaffold.TestBox.DecimalSeparator,
					_scaffold.TestBox.GroupSeparator
				)
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
			beforeInput = String.Format("123{1}456{1}789{1}123{1}457{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 24;
			input = String.Format("123{1}456{1}789{1}123{1}457{1}000{0}200", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 25;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 25);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{1}789{1}123{1}457{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));

			// 2
			beforeInput = String.Format("123{1}456{1}789{1}123{1}457{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			beforeInputCaretPosition = 25;
			input = String.Format("123{1}456{1}789{1}123{1}457{1}000{0}020", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			inputCaretPositionRef = 26;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 26);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{1}789{1}123{1}457{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}



		// TODO.it3xl.ru: Проверить, как поведут себя Decimal, Int32, UInt32 для огромных чисел.
	
	
	}
}
