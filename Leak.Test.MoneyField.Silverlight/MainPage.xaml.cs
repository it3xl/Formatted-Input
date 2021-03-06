﻿using System;
using System.Globalization;
using System.Threading;
using System.Windows.Threading;
using Leak.Test.MoneyField.Silverlight.Controller;

namespace Leak.Test.MoneyField.Silverlight
{
	public partial class MainPage
	{
		private readonly LeakLoadController _loadController = new LeakLoadController();

		private readonly DispatcherTimer _memoryCheckTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(2),
			};

		private readonly DispatcherTimer _aliveCheckTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1),
			};

		public MainPage()
		{
			InitializeComponent();

			_loadController.StartLoad(InjectionPlace);

			_memoryCheckTimer.Tick += _memoryCheckTimer_Tick;
			_memoryCheckTimer.Start();

			_aliveCheckTimer.Tick += _aliveCheckTimer_Tick;
			_aliveCheckTimer.Start();
		}

		void _memoryCheckTimer_Tick(object sender, EventArgs e)
		{
			SynchronizationContext.Current.Post(
				_ =>
				{
					var megabytesUsed = GC.GetTotalMemory(false) / (1024 * 1024);
					MegabytesUsedTextBox.Text = megabytesUsed.ToString(CultureInfo.InvariantCulture);
				},
				null
			);
		}

		void _aliveCheckTimer_Tick(object sender, EventArgs e)
		{
			AliveMarkTextBox.Text = DateTime.Now.ToString("HH:mm:ss");
		}

	}
}
