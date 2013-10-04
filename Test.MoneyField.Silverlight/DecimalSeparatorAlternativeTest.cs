using System.Threading;
using It3xl.FormattedInput.NullAndEmptyHandling;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class DecimalSeparatorAlternativeTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		/// <summary>
		/// The common behaviours for the DecimalSeparatorAlternative.
		/// </summary>
		[TestMethod]
		public void CommonBehaviours()
		{
			_scaffold.DoubleNullableMoneyTexBox.DecimalSeparatorAlternativeChar = ".";
			_scaffold.DoubleNullableMoneyTexBox.DecimalSeparatorChar = ",";
			_scaffold.DoubleNullableMoneyTexBox.GroupSeparatorChar = " ";

			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "9 876| 543.21".ToSpecificValue(out beforeInputCaretPosition);
			var input = "9 876^| 543.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "9 876.|54".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "9 876 |543.21".ToSpecificValue(out beforeInputCaretPosition);
			input = "9 876 ^|543.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "9 876.|54".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "9 876 5|43.21".ToSpecificValue(out beforeInputCaretPosition);
			input = "9 876 5^|43.21".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "98 765.|43".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345|.67".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345^|.67".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.|67".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.|67".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345.^|67".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.|67".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.6|7".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345.6^|7".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.6|7".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);


			beforeInput = "12 345.67|".ToSpecificValue(out beforeInputCaretPosition);
			input = "12 345.67^|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestFormatAndManageCaret(input, beforeInput, beforeInputCaretPosition, FocusEnum.HasNoState, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "12 345.67|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}


		/// <summary>
		/// Some initiating settings for the <see cref="NumberToMoneyConverter.DecimalSeparatorAlternative"/>.
		/// </summary>
		[TestMethod]
		public void InitiatingSettings()
		{
			var separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharFromFirst();
			var converter = new NumberToMoneyConverter
			{
				DecimalSeparator = separator,
				DecimalSeparatorAlternative = separator,
			};

			// It's allowable but not important behaviour.
			Assert.IsTrue(converter.DecimalSeparatorAlternative == converter.DecimalSeparator);
		}



	}
}
