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
		public void CommonBehaviours()
		{
			_scaffold.TestBox.AlternativeDecimalSeparator = '.';
			_scaffold.TestBox.DecimalSeparator = ',';
			_scaffold.TestBox.GroupSeparator = ' ';

			String formatteValueOut;

			var beforeInput = "9 876 543.21".ToSpecificValue();
			var beforeInputCaretPosition = 5;
			var input = "9 876_ 543.21".ToSpecificValue();
			var inputCaretPositionRef = 6;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 6);
			Assert.IsTrue(formatteValueOut == "9 876.54".ToSpecificValue());


			beforeInput = "9 876 543.21".ToSpecificValue();
			beforeInputCaretPosition = 6;
			input = "9 876 _543.21".ToSpecificValue();
			inputCaretPositionRef = 7;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 6);
			Assert.IsTrue(formatteValueOut == "9 876.54".ToSpecificValue());


			beforeInput = "9 876 543.21".ToSpecificValue();
			beforeInputCaretPosition = 7;
			input = "9 876 5_43.21".ToSpecificValue();
			inputCaretPositionRef = 8;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 7);
			Assert.IsTrue(formatteValueOut == "98 765.43".ToSpecificValue());


			beforeInput = "12 345.67".ToSpecificValue();
			beforeInputCaretPosition = 6;
			input = "12 345_.67".ToSpecificValue();
			inputCaretPositionRef = 7;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 7);
			Assert.IsTrue(formatteValueOut == "12 345.67".ToSpecificValue());


			beforeInput = "12 345.67".ToSpecificValue();
			beforeInputCaretPosition = 7;
			input = "12 345._67".ToSpecificValue();
			inputCaretPositionRef = 8;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 7);
			Assert.IsTrue(formatteValueOut == "12 345.67".ToSpecificValue());


			beforeInput = "12 345.67".ToSpecificValue();
			beforeInputCaretPosition = 8;
			input = "12 345.6_7".ToSpecificValue();
			inputCaretPositionRef = 9;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 8);
			Assert.IsTrue(formatteValueOut == "12 345.67".ToSpecificValue());


			beforeInput = "12 345.67".ToSpecificValue();
			beforeInputCaretPosition = 9;
			input = "12 345.67_".ToSpecificValue();
			inputCaretPositionRef = 10;

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == "12 345.67".ToSpecificValue());
		}


		// TODO.it3xl.com: 
		
		// Insertionf of a AlternativeDecimalSeparator
	
	}
}
