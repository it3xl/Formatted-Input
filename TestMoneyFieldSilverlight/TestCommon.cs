// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// HowTo:
// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
// Async HowTo:
// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing

namespace TestMoneyFieldSilverlight
{
	[TestClass]
	public class TestCommon : SilverlightTest
	{

		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		[TestMethod]
		public void AddTwoZeroForInteger()
		{
			_scaffold.ViewModel.AmountDouble = 50;

			Assert.IsTrue(_scaffold.TestBox.Text == String.Format("50{0}00", _scaffold.TestBox.DecimalSeparator));
		}

		[Ignore]
		[Description("Неправильный тест для демонстрации тестирования параллельной логики.")]
		[TestMethod]
		[Asynchronous]
		public void DummyParallelTesting()
		{
			_scaffold.TestBox.Text = String.Format("0{0}00", _scaffold.TestBox.DecimalSeparator);
			_scaffold.TestBox.SelectionStart = 1;

			//EnqueueConditional(() => true);

			//EnqueueDelay(TimeSpan.FromSeconds(5));

			EnqueueCallback(() => Assert.IsTrue(_scaffold.TestBox.SelectionStart == 1, String.Format("First Step: must be 1, but TestBox.SelectionStart == {0}", _scaffold.TestBox.SelectionStart)));
			EnqueueCallback(() => _scaffold.TestBox.Text = String.Format("1{0}00", _scaffold.TestBox.DecimalSeparator));

			//EnqueueDelay(TimeSpan.FromSeconds(5));
			
			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.TestBox.Text == String.Format("1{0}00", _scaffold.TestBox.DecimalSeparator));
					Assert.IsTrue(_scaffold.TestBox.SelectionStart == 1, String.Format("Second Step: must be 1, but TestBox.SelectionStart == {0}", _scaffold.TestBox.SelectionStart));
				});
			EnqueueTestComplete();
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

		/// <summary>
		/// Behavior checking for the input of a single not digit symbol at the empty field.
		/// </summary>
		[TestMethod]
		public void FirstIncorectSymbolBehavior()
		{
			var beforeInput = String.Empty;
			var beforeInputCaretPosition = 0;
			var input = "e";
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
			Assert.IsTrue(formatteValueOut == String.Format("0{0}00", _scaffold.TestBox.DecimalSeparator));
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

		/// <summary>
		/// The group delimiter test.
		/// </summary>
		[TestMethod]
		public void GroupDelimiterFormatting()
		{
			var beforeInput = "89";
			var beforeInputCaretPosition = 0;
			var input = "123456789";
			var inputCaretPositionRef = 7;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{1}789{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
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
		/// Decimal part cutting test.
		/// </summary>
		[TestMethod]
		public void EmptyValueIsEmptyField()
		{
			_scaffold.ViewModel.AmountDouble = null;

			Assert.IsTrue(_scaffold.TestBox.Text == String.Empty);
		}



		// TODO.it3xl.ru: 0000123 inserting with caret at the 3.


		// TODO.it3xl.ru: Как-то протестировать основные комбинации с основными разделителями и отступами в числах - Ru, En, Us.

		// TODO.it3xl.ru: Добавить поддержку Decimal, Int32, UInt32.
		// TODO.it3xl.ru: Проверить, как поведут себя Decimal, Int32, UInt32 для огромных чисел.


	}



}