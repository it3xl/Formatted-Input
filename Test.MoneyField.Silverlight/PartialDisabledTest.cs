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


		// TODO.it3xl.com: PartialDisabledTest:

		// Test of the ignoring of partial values from ViewModel on a get and on a set. 
		// Test main behaviours.

		/* Test cases for:
		 * IntegerBitness = 0;
		 * PartialPartDisabled = true;
		 * 
		 * 0|
		 * 01|
		 * 0|
		 * 
		 * |0
		 * 1|0
		 * |0
		 * 
		 * 0|
		 * 0.|
		 * 0|
		 * 
		 * |0
		 * .|0
		 * |0
		 * 
		 * 0|
		 * 012sdfjkls348sf.|
		 * 0|
		 * 
		 * |
		 * 1|
		 * 0
		 * 
		 * |
		 * d|
		 * |
		 * 
		 * 0|
		 * |
		 * |
		 * 
		 * |0
		 * |
		 * |
		 */

	}
}
