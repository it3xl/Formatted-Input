// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System.Globalization;
using System.Threading;
using It3xl.FormattedInput.View;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class GroupSeparatorTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		// TODO.it3xl.com: Test all existed cases with the GroupSeparator = Char.MinValue.



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

			_scaffold.TestBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "123 456 7|89.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Tests allowable of the empty value.
		/// </summary>
		[TestMethod]
		public void EmptyValueSupport()
		{
			var textBox = new MoneyTextBox
				{
					GroupSeparator = Char.MinValue
				};

			Assert.AreEqual(textBox.GroupSeparator, Char.MinValue);
		}
	
	}
}
