// ReSharper disable RedundantArgumentName

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
		/// Nullable{Double} Behaviors. The start value is empty.
		/// </summary>
		[TestMethod]
		public void NullableDoubleFromEmptyBehaviors()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "f|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdfl7slkej|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "7|.00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.lsdfj|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0.|00".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.ls3dfj|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0.3|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdl4kf.ls3dfj|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "4.3|0".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);

			beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			input = "sdlkf.ls3d1fj|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "0.31|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Nullable{Double} Behaviors. The start value is not empty.
		/// </summary>
		[TestMethod]
		[Tag("NullableDoubleToEmptyBehaviors")]
		public void NullableDoubleToEmptyBehaviors()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "1 234|.98".ToSpecificValue(out beforeInputCaretPosition);
			var input = "|".ToSpecificValue(out inputCaretPositionRef);
			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(FocusState.Gotten, input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);
			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Double Behaviors. The start value is not empty.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("DoubleToEmptyBehaviorsAsync")]
		public void DoubleToEmptyBehaviorsAsync()
		{
			Int32 beforeInputCaretPosition;
			_scaffold.DoubleMoneyTexBox.Text = "1234.98|".ToSpecificValue(out beforeInputCaretPosition);
			_scaffold.DoubleMoneyTexBox.SelectionStart = beforeInputCaretPosition;

			Int32 expectedCaretPosition;

			EnqueueCallback(() =>
			{
				// It imitates a input by a user and caret shouldn't jump to the integer end from the partial end.
				Assert.IsTrue(_scaffold.DoubleMoneyTexBox.Text == "1 234.98|".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleMoneyTexBox.SelectionStart == expectedCaretPosition);

				// It's the value which came from the ViewModel.
				_scaffold.ViewModel.DoubleMoney = 0;
			});

			//this.PrepareFocusFromDebugger(controlForFocus: _scaffold.DoubleMoneyTexBox);

			EnqueueCallback(() =>
			{
				Assert.IsTrue(_scaffold.DoubleMoneyTexBox.Text == "0|.00".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleMoneyTexBox.SelectionStart == expectedCaretPosition);
			});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Nullable Double Empty Behavior.
		/// </summary>
		[TestMethod]
		[Tag("NullableDoubleEmptyBehavior")]
		public void NullableDoubleEmptyBehavior()
		{
			Int32 beforeInputCaretPosition;
			Int32 inputCaretPositionRef;
			String formatteValueOut;
			Int32 expectedCaretPosition;

			var beforeInput = "|".ToSpecificValue(out beforeInputCaretPosition);
			var input = "|".ToSpecificValue(out inputCaretPositionRef);

			_scaffold.DoubleNullableMoneyTexBox.Converter.TestProcess(
				FocusState.JustGotten,
				input, beforeInput, beforeInputCaretPosition, out formatteValueOut, ref inputCaretPositionRef);

			Assert.IsTrue(formatteValueOut == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(inputCaretPositionRef == expectedCaretPosition);
		}

		/// <summary>
		/// Nullable Double Empty Behavior Async.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("NullableDoubleEmptyBehaviorAsync")]
		public void NullableDoubleEmptyBehaviorAsync()
		{
			Int32 expectedCaretPosition;

			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "|".ToSpecificValue(out expectedCaretPosition));
			Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);

			this.PrepareFocusFromDebugger(controlForFocus:_scaffold.DoubleNullableMoneyTexBox);

			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "|".ToSpecificValue(out expectedCaretPosition));
					Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
				});

			EnqueueTestComplete();
		}
	}
}
