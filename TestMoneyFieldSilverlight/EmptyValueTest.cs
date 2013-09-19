// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class EmptyValueTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		/// <summary>
		/// Behavior checking for the input of a single not digit symbol at the empty field.
		/// </summary>
		[TestMethod]
		public void FirstIncorectSymbolBehavior()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "e|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "0|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}
	
	
	}
}
