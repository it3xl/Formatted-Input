using System;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{

	[TestClass]
	public class DefaultAndEmptyValueTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Adding zero at the start is ignored and dosen't change the caret's position..
		/// </summary>
		[TestMethod]
		public void NullableDoubleBehaviors()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "f|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdfl7slkej|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "7|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.lsdfj|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "0.|00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.ls3dfj|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "0.3|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdl4kf.ls3dfj|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "4.3|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.ls3d1fj|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "0.31|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Adding zero at the start is ignored and dosen't change the caret's position..
		/// </summary>
		[TestMethod]
		public void DoubleBehaviors()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "f|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		// TODO.it3xl.com: DefaultValueTest
		/* Test cases for Double:
		 * 
		 * 0|.00
		 * |
		 * 0|.00|
		 * 
		 */

	
	}
}
