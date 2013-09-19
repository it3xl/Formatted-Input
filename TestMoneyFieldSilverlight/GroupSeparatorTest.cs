// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

namespace TestMoneyFieldSilverlight
{
	using System;
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class GroupSeparatorTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The group delimiter test.
		/// </summary>
		[TestMethod]
		public void GroupDelimiterFormatting()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|89.00".ToSpecificValue(out beforeInputCaretPosition);
			var input = "1234567|89.00".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 7|89.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}
	
	}
}
