// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	[TestClass]
	public class DelKeyTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}

		/// <summary>
		/// Deletion of the first digit before a bunch of zeros.
		/// </summary>
		[TestMethod]
		public void LeadingZeros()
		{
			var beforeInput = String.Format("80{1}000{1}009{1}123{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 0;
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
			var beforeInputCaretPosition = 0;
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
			var beforeInput = String.Format("80{1}111{1}222{1}333{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 4;
			var input = String.Format("80{1}11{1}222{1}333{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
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
			Assert.IsTrue(formatteValueOut == String.Format("8{1}011{1}222{1}333{0}25", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}


		/// <summary>
		/// Deletion of the Group Separator in the integer part of a number.
		/// </summary>
		[TestMethod]
		public void GroupSeparatorInMiddleDeletion()
		{
			var beforeInput = String.Format("80{1}111{1}222{1}333{1}444{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator);
			var beforeInputCaretPosition = 10;
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

			Assert.IsTrue(inputCaretPositionRef == 11);
			Assert.IsTrue(formatteValueOut == String.Format("8{1}011{1}122{1}233{1}444{1}000{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}
























		// TODO.it3xl.ru: The Del key.

		// В целой части должно удалять число справа.
		// Есил перед числом справа разделитель группы, то его удалять вместе с числом.
		// Тоже, но после удаления будет разделитель.
		// Все это повторить для первого и последнего разделителя.

		// Нажатие Del перед запятой переводит позицию курсора после запятой, ничего не меняя.
		// Есть ошибки:
		// Если после запятой числа, то они попадают в целую часть.
		// Если после запятой нули, то курсор скачет в нулевую позицию.

		// Del. Обработка удаления после запятой работает аналогично обычного текстового ввода, поэтому примем следующие правила
		// Удаление в позиции сразу после запятой удаляет первый символ после запятой передвигает на его метсто второй символ + оставляет курсор на прежнем месте.

		// Протестировать поведение GroupSeparator в разных случаях.
		// Протестировать поведение DecimalSeparator в разных случаях. Перенести сюда соответствующие тесты из DecimalSeparatorTest.

	}
}
