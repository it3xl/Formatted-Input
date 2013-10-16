using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class PartialBitnessTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		// It's a huge TO-DO. Defenetly, I am not planning to do it now.
		// If I'll implement this I could delete the PartialDisabled and I should think about the PartialDisabledCurrent for sake of PartialBitness == 0;

		
	}
}
