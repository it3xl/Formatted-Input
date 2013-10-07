using It3xl.Test.MoneyField.Silverlight.Utils;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace It3xl.Test.MoneyField.Silverlight
{
	[TestClass]
	public class IntegerBitnessTest : SilverlightTest
	{
		private readonly LocalScaffold _scaffold = new LocalScaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}



		// it3xl.com: I decided that the integer bitness it's the matter of the MVVM validation.
		// Just in case I change my minde I left the test cases here.
		/* Test cases:
		 * 
		0
		0.76
		0|.76
		0.|76
		 * 
		0
		0.76
		|0.76
		0.|76
		 * 
		0
		0.76
		0.76|
		0.|76
		 * 
		 * 
		5
		12345|.76
		123456|.76
		12345|.76
		 * 
		5
		1234|5.76
		12348|5.76
		12348|.76
		 * 
		5
		1|2345.76
		18|2345.76
		18|234.76
		 * 
		5
		|12345.76
		8|12345.76
		8|1234.76
		 * 
		 * 
		3
		1|67.12
		12345|67.12
		123|.12
		 * 
		3
		1|67.12
		1234567.89|67.12
		123.89|
		 * 
		3
		1|67.12
		1234567.8|67.12
		123.8|6
		 * 
		3
		1|89.12
		1234567.|89.12
		123.|89
		 * 
		3
		1|89.12
		1234567|89.12
		123|.12
		 * 
		5
		1|56.12
		1234|56.12
		1234|5.12
		 * 
		*/
		/* Test cases for Nullable<Double>:
		 * 
		 * IntegerBitness == 0
		 * |
		 * sdfl7slkej|
		 * 0.|00
		 * 
		 * IntegerBitness == 0
		 * |
		 * sdl4kf.ls3dfj|
		 * 0.3|0
		 * 
		 */
		/* Test cases for Double:
		 * 
		 * IntegerBitness == 0
		 * 0.|00
		 * |
		 * 0.|00
		 * 
		 */

	}
}
