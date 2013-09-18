﻿// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

	[TestClass]
	public class NullableTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The Double Type default value behavior.
		/// </summary>
		[TestMethod]
		public void NullableDoubleIsEmptyField()
		{
			_scaffold.ViewModel.AmountDouble = null;

			Assert.IsTrue(_scaffold.TestBox_.Text == String.Empty);
		}



		// TODO.it3xl.com: Добавить поддержку Decimal, Int32, UInt32.


	}
}
