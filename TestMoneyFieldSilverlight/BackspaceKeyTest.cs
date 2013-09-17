// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
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
		/// Decimal Separator deletion.
		/// </summary>
		[TestMethod]
		public void DecimalSeparatorDeletion()
		{
			var beforeInput = String.Format("80{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 1;
			var input = String.Format("0{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == String.Format("9{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingZeros()
		{
			var beforeInput = String.Format("80{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 1;
			var input = String.Format("0{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == String.Format("9{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingSeparator()
		{
			var beforeInput = String.Format("8{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 1;
			var input = String.Format("{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var inputCaretPositionRef = 0;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 0);
			Assert.IsTrue(formatteValueOut == String.Format("9{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// Deletion of a digit in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void IntegerDigitDeletion()
		{
			var beforeInput = String.Format("80{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 5;
			var input = String.Format("80{1}00{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
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
			Assert.IsTrue(formatteValueOut == String.Format("8{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}

		/// <summary>
		/// Deletion of the Group Separator in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorInMiddleDeletion()
		{
			var beforeInput = String.Format("80{1}111{1}222{1}333{1}444{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 11;
			var input = String.Format("80{1}111{1}222333{1}444{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var inputCaretPositionRef = 10;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == String.Format("8{1}011{1}122{1}333{1}444{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}






























		// TODO.it3xl.ru: The Backspace key.

		// В целой части должно удалять число слева.
		// Есил перед числом слева разделитель группы, то его удалять вместе с числом. Куроср поставить перед разделителем.
		// Тоже, но после удаления будет разделитель.
		// Все это повторить для первого и последнего разделителя.
		
		// Ошибка! Удаление разделителя ведет к умножению целой части на 100.
		// Нужно, чтоб ничего не менялось, а позиция курсора вставала перед запятой.

		// Backspace в дробной части должен отрабатывать, как стрелка влево с обнулением чисел.
		// Удаление запятой кнопкой Backspace.
		// 12,00
		// 0,00 - курсор должен оставаться в позиции 1 (т.е. Backspace должен отработать, как стрелка влево)
		// 0,00 - если курсор в первой позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.
		// 0,00 - если курсор в 0 позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.

		// Протестировать поведение GroupSeparator в разных случаях.
		// Протестировать поведение DecimalSeparator в разных случаях. Перенести сюда соответствующие тесты из DecimalSeparatorTest.

	}
}
