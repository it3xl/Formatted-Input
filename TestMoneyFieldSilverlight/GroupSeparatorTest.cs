// ReSharper disable ConvertToConstant.Local
// ReSharper disable JoinDeclarationAndInitializer

using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMoneyFieldSilverlight
{
	[TestClass]
	public class GroupSeparatorTest : SilverlightTest
	{
		private readonly Scaffold _scaffold = new Scaffold();

		[TestInitialize]
		public void TestInitialize()
		{
			_scaffold.TestInitialize(TestPanel);
		}


		/// <summary>
		/// The group delimiter test.
		/// </summary>
		[TestMethod]
		public void GroupDelimiterFormatting()
		{
			var beforeInput = "89";
			var beforeInputCaretPosition = 0;
			var input = "123456789";
			var inputCaretPositionRef = 7;

			String formatteValueOut;

			_scaffold.TestBox.Converter.FormatDoubleManagePosition(
				input,
				beforeInput,
				beforeInputCaretPosition,
				out formatteValueOut,
				ref inputCaretPositionRef
			);

			Assert.IsTrue(inputCaretPositionRef == 9);
			Assert.IsTrue(formatteValueOut == String.Format("123{1}456{1}789{0}00", _scaffold.TestBox.DecimalSeparator, _scaffold.TestBox.GroupSeparator));
		}


		// TODO.it3xl.ru: .
		// Separator inserting ignored an any position.
	
	}
}
