using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using It3xl.Scaffold.MoneyField.Silverlight.ViewModel;
using Leak.Test.MoneyField.Silverlight.View;

namespace Leak.Test.MoneyField.Silverlight.Controller
{
	public class LeakLoadController
	{
		private readonly DispatcherTimer _loadTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(1000),
		};

		private Grid _injectionElement;

		private static readonly ViewModelForTests ViewModel = new ViewModelForTests
			{
				DoubleNullableMoney = 1.11,
				DoubleMoney = 2.22,
				DecimalNullableMoney = 3.33M,
				DecimalMoney = 4.44M,
			};

		internal void StartLoad(Grid injectionPlace)
		{
			_injectionElement = injectionPlace;

			_loadTimer.Tick += _loadTimer_Tick;
			_loadTimer.Start();
		}

		void _loadTimer_Tick(object sender, EventArgs e)
		{
			SynchronizationContext.Current.Post(
				_ => SetLoad(_injectionElement),
				null
			);
		}

		private static void SetLoad(Grid injectionElement)
		{
			injectionElement.Children.Clear();

			var bunchControl = new BunchControl();

			injectionElement.Children.Add(bunchControl);

			bunchControl.DataContext = ViewModel;
		}
	}
}
