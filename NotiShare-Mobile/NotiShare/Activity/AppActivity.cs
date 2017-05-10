using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Interop;
using NotiShare.Helper;
using NotiShareModel.DataTypes;
using NotiShareModel.HttpWorker;

namespace NotiShare.Activity
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "@string/Action", Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
    public class AppActivity : AppCompatActivity
    {

        private RelativeLayout progressLayout;
        private LinearLayout mainLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.app_layout);
            progressLayout = FindViewById<RelativeLayout>(Resource.Id.progressBar);
            mainLayout = FindViewById<LinearLayout>(Resource.Id.mainLayout);
        }

        public override async void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            var deviceObject = new RegisterDeviceObject
            {
                DeviceId = Build.Serial,
                Email = AppHelper.ReadString("email", String.Empty, this),
                DeviceType = "droid"
            };
            var result = await HttpWorker.Instance.RegisterDevice(deviceObject);
            AppHelper.ShowToastText(this, result);
            progressLayout.Visibility = ViewStates.Gone;
            mainLayout.Visibility = ViewStates.Visible;
        }




        [Export("onLogout")]
        public void Logout(View view)
        {
            AppHelper.ClearValue("email", this);
            AppHelper.ClearValue("password", this);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        [Export("onSettings")]
        public void OpenSettings(View view)
        {
            var intent = new Intent(this, typeof(SettingsActivity));
            StartActivity(intent);
        }
    }
}