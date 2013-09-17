// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	[TestClass]
	public class TestBackspaceKey : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}






		// TODO.it3xl.ru: The Backspace key.


		// TODO.it3xl.ru: Deleting the 8 at the 80000123.

		// В целой части должен удалять число слева.
		// Есил перед числом слева разделитель группы, то его удалять вместе с числом. Куроср поставить перед разделителем.

		// Ошибка! Удаление разделителя ведет к умножению целой части на 100.
		// Нужно, чтоб ничего не менялось, а позиция курсора вставала перед запятой.

		// Backspace в дробной части должен отрабатывать, как стрелка влево с обнулением чисел.
		// Удаление запятой кнопкой Backspace.
		// 12,00
		// 0,00 - курсор должен оставаться в позиции 1 (т.е. Backspace должен отработать, как стрелка влево)
		// 0,00 - если курсор в первой позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.
		// 0,00 - если курсор в 0 позиции и нажимется Backspace, то нужно ставить курсор в позицию 1.

	}
}
