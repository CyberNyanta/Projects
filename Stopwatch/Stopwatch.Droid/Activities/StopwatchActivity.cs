using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Stopwatch.Core.Utils;
using Android.Animation;
using Android.Graphics.Drawables;
using Android.Util;
using Stopwatch.Droid.Services;

namespace Stopwatch.Droid.Activities
{
	[Activity(Label = "StopwatchActivity", Icon = "@drawable/icon")]
	public class StopwatchActivity : Activity
	{
		#region NestedClasses

		class StopwatchServiceConnection : Java.Lang.Object, IServiceConnection
		{
			StopwatchActivity activity;
			StopwatchServiceBinder binder;

			public StopwatchServiceBinder Binder
			{
				get
				{
					return binder;
				}
			}

			public StopwatchServiceConnection(StopwatchActivity activity)
			{
				this.activity = activity;
			}

			public void OnServiceConnected(ComponentName name, IBinder service)
			{
				var stopwatchServiceBinder = service as StopwatchServiceBinder;
				if (stopwatchServiceBinder != null)
				{
					var binder = (StopwatchServiceBinder)service;
					activity.binder = binder;
					activity.isBound = true;

					// keep instance for preservation across configuration changes
					this.binder = (StopwatchServiceBinder)service;
				}
			}

			public void OnServiceDisconnected(ComponentName name)
			{
				activity.isBound = false;
			}
		}
		class ClickListener : Java.Lang.Object, View.IOnClickListener
		{
			StopwatchActivity instance;
			public ClickListener(StopwatchActivity instance)
			{
				this.instance = instance;
			}

			public void OnClick(View v)
			{
				instance.expanded = !(instance.expanded);
				if (instance.expanded)
				{
					instance.expandFab();
				}
				else
				{
					instance.collapseFab();
				}
			}
		}

		class PreDrawListener : Java.Lang.Object, ViewTreeObserver.IOnPreDrawListener
		{

			StopwatchActivity instance;
			public PreDrawListener(StopwatchActivity instance)
			{
				this.instance = instance;
			}
			public bool OnPreDraw()
			{

				instance.fabContainer.ViewTreeObserver.RemoveOnPreDrawListener(this);
				instance.offset1 = instance.fab.GetY() - instance.fabPause.GetY();
				instance.fabPause.TranslationY = instance.offset1;
				instance.offset2 = instance.fab.GetY() - instance.fabStop.GetY();
				instance.fabStop.TranslationY = instance.offset2;
				instance.offset3 = instance.fab.GetY() - instance.fabReload.GetY();
				instance.fabReload.TranslationY = instance.offset3;
				return true;
			}
		}
		#endregion

		#region prop
		const String TAG = "Floating Action Button";
		const String TRANSLATION_Y = "translationY";
		internal ImageButton fab;
		internal bool expanded;
		internal ImageButton fabPause;
		internal ImageButton fabStop;
		internal ImageButton fabReload;
		internal float offset1;
		internal float offset2;
		internal float offset3;
		LinearLayout startlayout;
		ImageButton startButton;
        TextView chronometer;

		internal ViewGroup fabContainer;
		long startTime;
        long elapsedOnPause;
        bool pauseClicked = false; 

        private long ElapsedTime { get { return (SystemClock.ElapsedRealtime() - startTime); } }

        StopwatchServiceBinder binder;
		StopwatchServiceConnection stopwatchServiceConnection;
		bool isBound = false;


		#endregion

		protected override void OnCreate(Bundle instance)
		{
			base.OnCreate(instance);
			SetContentView(Resource.Layout.Stopwatch);

			fabContainer = (ViewGroup)FindViewById(Resource.Id.fab_container);
			fab = (ImageButton)FindViewById(Resource.Id.fab);
			fabPause = FindViewById<ImageButton>(Resource.Id.fab_pause);
            fabStop = FindViewById<ImageButton>(Resource.Id.fab_stop);
            fabReload = FindViewById<ImageButton>(Resource.Id.fab_reload);
			chronometer = FindViewById<TextView>(Resource.Id.textClock);

			startlayout = FindViewById<LinearLayout>(Resource.Id.start_layout);
			var userName = FindViewById<TextView>(Resource.Id.user_name);
			userName.Text = Intent.GetStringExtra("login");
            chronometer.Text = "00:00";
            startButton = FindViewById<ImageButton>(Resource.Id.start_button);

			fab.SetOnClickListener(new ClickListener(this));

			fabContainer.ViewTreeObserver.AddOnPreDrawListener(new PreDrawListener(this));
            

			startButton.Click += delegate
			{
				StartButtonClick();
			};
            fabPause.Click += delegate
			{
				OnTimerPause();
			};
            fabStop.Click += delegate
			{
                OnTimerStop();
			};
            fabReload.Click += delegate
			{
                OnTimerReload();
			};



            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            SetActionBar(toolbar);

            ActionBar.Title = "Hello from Toolbar";

           


        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.stopwatchMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_zero:
                    OnBackPressed();
                    break;
                case Resource.Id.menu_about:
                    var activity = new Intent(this, typeof(AboutActivity));                  
                    StartActivity(activity);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnStart()
		{
			base.OnStart();

			Intent stopwatchServiceIntent = new Intent("com.xamarin.StopwatchService");
			stopwatchServiceConnection = new StopwatchServiceConnection(this);
			ApplicationContext.BindService(stopwatchServiceIntent, stopwatchServiceConnection, Bind.AutoCreate);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

            if (isBound)
            {
                
                ApplicationContext.UnbindService(stopwatchServiceConnection);
                isBound = false;
               
            }
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutLong("startTime", startTime);



        }
        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(savedInstanceState);

			if (savedInstanceState != null)
			{
				startlayout.Visibility = ViewStates.Gone;
				fabContainer.Visibility = ViewStates.Visible;
				chronometer.Visibility = ViewStates.Visible;
				startTime = savedInstanceState.GetLong("startTime");


				
			}
		}



		#region Actions
		public void OnTimerPause()
		{
            if (!pauseClicked)
            {
                pauseClicked = true;
                elapsedOnPause = ElapsedTime;

                fabPause.SetImageResource(Resource.Drawable.play);
                stopwatchServiceConnection.Binder.GetStopwatchService().PauseTimer();
            }
            else
            {
                pauseClicked = false;
                if (elapsedOnPause != 0)
                {
                    startTime += ElapsedTime - elapsedOnPause;                
                }
                else
                {
                    startTime = SystemClock.ElapsedRealtime();
                }
                
                fabPause.SetImageResource(Resource.Drawable.pause);
                stopwatchServiceConnection.Binder.GetStopwatchService().ResumeTimer();
            }
		}

		public void OnTimerStop()
		{
            stopwatchServiceConnection.Binder.GetStopwatchService().StopTimer();
            chronometer.Text = "00:00";
            startlayout.Visibility = ViewStates.Visible;
            fabContainer.Visibility = ViewStates.Invisible;
            chronometer.Visibility = ViewStates.Invisible;
            collapseFab();
            pauseClicked = false;
        }

		public void OnTimerReload()
		{
            chronometer.Text = "00:00";
            startTime = SystemClock.ElapsedRealtime();
            elapsedOnPause = 0;

        }

		public void StartButtonClick()
		{
			startTime = SystemClock.ElapsedRealtime();
			StartService(new Intent("com.xamarin.StopwatchService"));
			stopwatchServiceConnection.Binder.GetStopwatchService().StartTimer(1000, () => OnTimerTick());

            startlayout.Visibility = ViewStates.Gone;
            fabContainer.Visibility = ViewStates.Visible;
            chronometer.Visibility = ViewStates.Visible;
        }

		private void OnTimerTick()
		{

			
            TimeSpan ts = TimeSpan.FromMilliseconds(ElapsedTime);
            string elapsedTime = ts.ToString(@"mm\:ss");

            SendNotification(1, elapsedTime, null, Resource.Drawable.StartButton); //need refactoring

            
            RunOnUiThread(() =>
            {
                chronometer.Text = elapsedTime;
            });
            
        }


		public void SendNotification(int id,string title, string text, int icon)
		{
			Notification.Builder builder = new Notification.Builder(this);
			builder.SetContentTitle(title);
			builder.SetContentText(text);
			builder.SetSmallIcon(icon);
            

			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager =
				GetSystemService(Context.NotificationService) as NotificationManager;

			// Publish the notification:
			notificationManager.Notify(id, notification);
		}
		#endregion

		#region Animations
		private void collapseFab()
		{
			fab.SetImageResource(Resource.Drawable.animated_minus);
			AnimatorSet animatorSet = new AnimatorSet();
			animatorSet.PlayTogether(createCollapseAnimator(fabPause, offset1),
					createCollapseAnimator(fabStop, offset2),
					createCollapseAnimator(fabReload, offset3));
			animatorSet.Start();
			animateFab();
		}

		private void expandFab()
		{
			fab.SetImageResource(Resource.Drawable.animated_plus);
			AnimatorSet animatorSet = new AnimatorSet();
			animatorSet.PlayTogether(createExpandAnimator(fabPause, offset1),
					createExpandAnimator(fabStop, offset2),
					createExpandAnimator(fabReload, offset3));
			animatorSet.Start();
			animateFab();
		}

		private Animator createCollapseAnimator(View view, float offset)
		{
			return ObjectAnimator.OfFloat(view, TRANSLATION_Y, 0, offset)
					.SetDuration(Resources.GetInteger(Android.Resource.Integer.ConfigMediumAnimTime));
		}

		private Animator createExpandAnimator(View view, float offset)
		{
			return ObjectAnimator.OfFloat(view, TRANSLATION_Y, offset, 0)
					.SetDuration(Resources.GetInteger(Android.Resource.Integer.ConfigMediumAnimTime));
		}

		private void animateFab()
		{
			Drawable drawable = fab.Drawable;
			if (drawable is IAnimatable)
			{
				((IAnimatable)drawable).Start();
			}
		}
		#endregion





	}
}