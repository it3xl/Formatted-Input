// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class AlternateDecimalSeparatorTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		/// <summary>
		/// 
		/// </summary>
		[TestMethod]
		public void X()
		{
			//String formatteValueOut;

			//var beforeInput = String.Empty;
			//var beforeInputCaretPosition = 0;
			//var input = "e";
			//var inputCaretPositionRef = 1;

			//_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			//Assert.IsTrue(inputCaretPositionRef == 1);
			//Assert.IsTrue(formatteValueOut == "0.00".ToSpecificValue());
		}


		// TODO.it3xl.com: 
		
		// Insertionf of a AlternativeDecimalSeparator
	
	}
}
