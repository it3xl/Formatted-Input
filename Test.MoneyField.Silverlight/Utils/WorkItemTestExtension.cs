using System;
using System.Diagnostics;
using System.Windows.Controls;
using It3xl.FormattedInput.NullAndEmptyHandling;
using Microsoft.Silverlight.Testing;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{
	public static class WorkItemTestExtension
	{
		/// <summary>
		/// Repairs focus tests started under a debugger (Visual Studio).
		/// </summary>
		/// <param name="scaffold"></param>
		/// <param name="controlForFocus"></param>
		public static void PrepareFocusFromDebugger(this WorkItemTest scaffold, Control controlForFocus)
		{
			if(Debugger.IsAttached
				&& scaffold.UnitTestHarness.Parameters["tagExpression"].IsNotNullOrEmpty()
				)
			{
				// You have 5 seconds to set the focus from Visual Studio to the testing window.
				// Otherwise a test with the Control.Focus() don't work.
				// + And set a first breakpoint not earlier than the first Assert statement.
				// (Cause: Visual Studio kills the focus in time of a debugging.)
				Debugger.Break();
				scaffold.EnqueueDelay(TimeSpan.FromSeconds(5));
			}

			scaffold.EnqueueCallback(() => controlForFocus.Focus());
		}
	
	}
}