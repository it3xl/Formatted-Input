using System;
using System.Diagnostics;
using It3xl.FormattedInput.View.Converter;
using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class PartialDisabledOnInput : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// Tests main behaviors.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("MainBehavioursAsync")]
		public void MainBehavioursAsync()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = false;
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabledOnInput = true;

			_scaffold.ViewModel.DoubleNullableMoney = 12345.74;

			this.PrepareFocusFromDebugger(_scaffold.DoubleNullableMoneyTexBox);
			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 345|".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 12345.0);

				// ensure the "LostFocus" for the DoubleNullableMoneyTexBox.
				_scaffold.DoubleMoneyTexBox.Focus();
			});

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 345|.00".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 12345.0);
			});

			EnqueueTestComplete();
		}

		/// <summary>
		/// Tests seting from a ViewModel for the PartialDisabledOnInput = true;
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("ForViewModelAsync")]
		public void ForViewModelAsync()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = false;
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabledOnInput = true;

			_scaffold.ViewModel.DoubleNullableMoney = 12345.74;

			EnqueueCallback(() =>
			{
				Int32 expectedCaretPosition;

				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "12 345|.00".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(expectedCaretPosition == _scaffold.DoubleNullableMoneyTexBox.SelectionStart);
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == 12345.0);
			});

			EnqueueTestComplete();
		}


		/// <summary>
		/// EmptyTextForDouble behaviour.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("EmptyTextForDoubleAsync")]
		public void EmptyTextForDoubleAsync()
		{
			_scaffold.DoubleMoneyTexBox.PartialDisabled = false;
			_scaffold.DoubleMoneyTexBox.PartialDisabledOnInput = true;

			_scaffold.ViewModel.DoubleMoney = 0;

			this.PrepareFocusFromDebugger(_scaffold.DoubleMoneyTexBox);

			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.DoubleMoneyTexBox.Text == "0");

					_scaffold.DoubleMoneyTexBox.Text = String.Empty;
				});

			EnqueueCallback(() =>
			{
				Assert.IsTrue(_scaffold.ViewModel.DoubleMoney == 0);
				
				if(Debugger.IsAttached == false)
				{
					Int32 expectedCaretPosition;
					Assert.IsTrue(_scaffold.DoubleMoneyTexBox.Text == "|0".ToSpecificValue(out expectedCaretPosition));
					Assert.IsTrue(_scaffold.DoubleMoneyTexBox.SelectionStart == expectedCaretPosition);
				}
			});

			EnqueueTestComplete();
		}


		/// <summary>
		/// EmptyTextForNullableDouble behaviour.
		/// </summary>
		[TestMethod]
		[Asynchronous]
		[Tag("EmptyTextForNullableDouble")]
		public void EmptyTextForNullableDouble()
		{
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabled = false;
			_scaffold.DoubleNullableMoneyTexBox.PartialDisabledOnInput = true;

			_scaffold.ViewModel.DoubleNullableMoney = 2;

			this.PrepareFocusFromDebugger(_scaffold.DoubleNullableMoneyTexBox);

			EnqueueCallback(() =>
				{
					Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "2");

					_scaffold.DoubleNullableMoneyTexBox.Text = String.Empty;
				});

			EnqueueCallback(() =>
			{
				Assert.IsTrue(_scaffold.ViewModel.DoubleNullableMoney == null);
				
				Int32 expectedCaretPosition;
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.Text == "|".ToSpecificValue(out expectedCaretPosition));
				Assert.IsTrue(_scaffold.DoubleNullableMoneyTexBox.SelectionStart == expectedCaretPosition);
			});

			EnqueueTestComplete();
		}

	}
}
