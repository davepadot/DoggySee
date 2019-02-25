using System;
using Android.App;
using Android.Hardware;
using Android.Media;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace DoggySee
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ISensorEventListener
    {

        private SensorManager _sensorManager;
        private MediaPlayer _mediaPlayer;
        private DateTime _lastBeep;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            _sensorManager = (SensorManager)this.GetSystemService(SensorService);
            _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.beep);

            if (_sensorManager.GetSensorList(SensorType.Proximity).Count != 0)
            {
                var proximitySensor = _sensorManager.GetDefaultSensor(SensorType.Proximity);
                _sensorManager.RegisterListener(this, proximitySensor, SensorDelay.Normal);
            }

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            //todo
        }

        public void OnSensorChanged(SensorEvent e)
        {
            Beep();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Send questions to davepadot@gmail.com", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool Beep()
        {
            if (DateTime.Now.Subtract(_lastBeep).TotalMilliseconds > 1200)
                _mediaPlayer.Start();
            _lastBeep = DateTime.Now;
            return true;
        }
    }
}

