using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace Stopwatch.Droid.Services
{
	[Service]
	[IntentFilter(new String[] { "com.xamarin.StopwatchService" } )]
	public class StopwatchService : Service
	{
		Timer timer;
		StopwatchServiceBinder binder;



		public override IBinder OnBind(Intent intent)
		{
			binder = new StopwatchServiceBinder(this);
			return binder;
		}

		public void StartTimer(long interval, Action callback)
		{
			timer = new Timer(interval);
			timer.Elapsed += (sender, e) => callback();
			timer.AutoReset = true;
			timer.Start();
		}

        public void PauseTimer()
        {
            timer.Stop();            
        }
        public void StopTimer()
        {
            timer?.Dispose();
        }

        public void ResumeTimer()
        {
            timer.Start();
        }


        [return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			
			return StartCommandResult.RedeliverIntent;

		
		}

        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
            StopSelf();
        }


        public override bool OnUnbind(Intent intent)
        {
            timer?.Dispose();
            return base.OnUnbind(intent);
        }
    }

	public class StopwatchServiceBinder : Binder
	{
		StopwatchService service;

		public StopwatchServiceBinder(StopwatchService service)
		{
			this.service = service;
		}

		public StopwatchService GetStopwatchService()
		{
			return service;
		}
	}
}