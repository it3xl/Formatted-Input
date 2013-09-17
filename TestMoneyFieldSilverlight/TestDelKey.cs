// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using System.Globalization;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	[TestClass]
	public class TestDelKey : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}




		// Проверить, что этот тест есть: Ввод после запятой должен сдвигать прежний первый символ вправо. Курсор передвигается на 1 символ.





		// TODO.it3xl.ru: The Del key.

		// TODO.it3xl.ru: Deleting the 8 at the 80000123.

		// В целой части должно удалять число справа.
		// Есил перед числом справа разделитель группы, то его удалять вместе с числом.

		// Нажатие Del перед запятой переводит позицию курсора после запятой, ничего не меняя.
		// Есть ошибки:
		// Если после запятой числа, то они попадают в целую часть.
		// Если после запятой нули, то курсор скачет в нулевую позицию.

		// Del. Обработка удаления после запятой работает аналогично обычного текстового ввода, поэтому примем следующие правила
		// Удаление в позиции сразу после запятой удаляет первый символ после запятой передвигает на его метсто второй символ + оставляет курсор на прежнем месте.


	}
}
