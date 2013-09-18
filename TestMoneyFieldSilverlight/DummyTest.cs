// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// HowTo:
// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
// Async HowTo:
// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing

namespace TestMoneyFieldSilverlight
{
	using TestMoneyFieldSilverlight.Utils;

	[TestClass]
	public class DummyTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// It's the useless test for the demonstration of a parallel testing.
		/// </summary>
		[Ignore]
		[Description("Useless test to demonstrate the parallel testing.")]
		[TestMethod]
		[Asynchronous]
		public void DummyParallelTesting()
		{
			_scaffold.TestBox_.Text = "0.00".ToSpecificValue();
			_scaffold.TestBox_.SelectionStart = 1;

			//EnqueueConditional(() => true);

			//EnqueueDelay(TimeSpan.FromSeconds(5));

			EnqueueCallback(() => Assert.IsTrue(_scaffold.TestBox_.SelectionStart == 1, String.Format("First Step: must be 1, but TestBox.SelectionStart == .", _scaffold.TestBox_.SelectionStart)));
			EnqueueCallback(() => _scaffold.TestBox_.Text = "1.00".ToSpecificValue());

			//EnqueueDelay(TimeSpan.FromSeconds(5));

			EnqueueCallback(() =>
			{
				Assert.IsTrue(_scaffold.TestBox_.Text == "1.00".ToSpecificValue());
				Assert.IsTrue(_scaffold.TestBox_.SelectionStart == 1, String.Format("Second Step: must be 1, but TestBox.SelectionStart == .", _scaffold.TestBox_.SelectionStart));
			});
			EnqueueTestComplete();
		}
	
	}
}
