using System;
using System.Diagnostics;
using Microsoft.Silverlight.Testing;

namespace It3xl.Test.MoneyField.Silverlight.Utils
{
	public static class WorkItemTestExtension
	{
		public static void PrepareSetFocusFromDebugger(this WorkItemTest scaffold)
		{
			if(Debugger.IsAttached == false)
			{
				return;
			}

			// You have 5 seconds to set the focus from Visual Studio to the test window.
			// Otherwise a test with the Control.Focus() don't work.
			// + And set a first breakpoint not earlier than the first Assert statement.
			Debugger.Break();

			scaffold.EnqueueDelay(TimeSpan.FromSeconds(5));
		}
	
	}
}