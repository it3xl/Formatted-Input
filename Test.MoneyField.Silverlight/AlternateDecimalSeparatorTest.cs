// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class AlternateDecimalSeparatorTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		/// <summary>
		/// The common behaviours for the AlternativeDecimalSeparator.
		/// </summary>
		[TestMethod]
		public void CommonBehaviours()
		{
			_scaffold.TestBox.AlternativeDecimalSeparator = '.';
			_scaffold.TestBox.DecimalSeparator = ',';
			_scaffold.TestBox.GroupSeparator = ' ';

			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "9 876| 543.21".ToSpecificValue(out beforeInputCaretPosition);
			var input = "9 876_| 543.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "9 876.|54".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "9 876 |543.21".ToSpecificValue(out beforeInputCaretPosition);
			input = "9 876 _|543.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "9 876.|54".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "9 876 5|43.21".ToSpecificValue(out beforeInputCaretPosition);
			input = "9 876 5_|43.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "98 765.|43".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345|.67".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345_|.67".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.|67".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.|67".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345._|67".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.|67".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.6|7".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345.6_|7".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.6|7".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.67|".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345.67_|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.FormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.67|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}


	}
}
