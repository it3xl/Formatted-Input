// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

// HowTo:
// http://www.jeff.wilcox.name/2008/03/silverlight2-unit-testing/
// Async HowTo:
// http://developer.yahoo.com/dotnet/silverlight/2.0/unittest.html#async
// http://stackoverflow.com/questions/11513422/silverlight-async-unit-testing

namespace TestMoneyFieldSilverlight
{
	using Microsoft.Silverlight.Testing;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Utils;

	[TestClass]
	public class CuttingOrDeletionTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		// TODO.it3xl.com: Make any tests.
	}
}
