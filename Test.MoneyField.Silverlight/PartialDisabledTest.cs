using System;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class PartialDisabledTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Tests the PartialDisabled main behaviors.
		/// </summary>
		[TestMethod]
		public void PartialDisabled()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = true;

			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "01|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0".ToSpecificValue(out beforeInputCaretPosition);
			input = "1|0".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "0.|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0".ToSpecificValue(out beforeInputCaretPosition);
			input = ".|0".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "012sdfjkls348sf.|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "12 348|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "1|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "1|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "d|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "0|".ToSpecificValue(out beforeInputCaretPosition);
			input = "|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|0".ToSpecificValue(out beforeInputCaretPosition);
			input = "|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, RuntimeType.Double, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Tests ignoring of the partial part of a number from a ViewMode and replacing it by the zero.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		public void PartialIgnoringFromViewModeAsync()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = true;
			_scaffold.ViewModel.DoubleNullableMoney = 12345.74;

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 345|".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 12345.0);
			});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests seting from a ViewModel for the PartialDisabled = true;
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("PartialDisabledForViewModel")]
		public void PartialDisabledForViewModelAsync()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = true;
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabledOnInput = false;

			_scaffold.ViewModel.DoubleNullableMoney = 12345.74;

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 345|".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 12345.0);
			});

			EnqueueTestComplete();
		}

	}
}
