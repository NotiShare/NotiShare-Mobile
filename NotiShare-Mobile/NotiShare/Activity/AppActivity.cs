﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Interop;
using NotiShare.Helper;
using NotiShareModel.DataTypes;
using NotiShareModel.HttpWorker;
using AlertDialog = Android.App.AlertDialog;

namespace NotiShare.Activity
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Label = "@string/Action")]
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
            if (!IsHaveSerial())
            {
                AppHelper.ClearValue(PreferenceKeys.LoginKey, this);
                AppHelper.ClearValue(PreferenceKeys.PasswordHash, this);
                return;
            }
            var deviceObject = new RegisterDeviceObject
            {
                DeviceId = Build.Serial,
                UserId = AppHelper.ReadInt(PreferenceKeys.UserIdKey, this, -1),
                DeviceType = 1,
                DeviceName = Build.Model
            };
            var result = await HttpWorker.Instance.RegisterDevice(deviceObject);
            AppHelper.ShowToastText(this, result.Message);
            AppHelper.WriteInt(PreferenceKeys.UserDeviceId, result.UserDeviceDbId, this);
            progressLayout.Visibility = ViewStates.Gone;
            mainLayout.Visibility = ViewStates.Visible;
        }




        [Export("onLogout")]
        public void Logout(View view)
        {
            AppHelper.ClearValue(PreferenceKeys.LoginKey, this);
            AppHelper.ClearValue(PreferenceKeys.PasswordHash, this);
            AppHelper.ClearValue(PreferenceKeys.UserDeviceId, this);
            AppHelper.ClearValue(PreferenceKeys.UserIdKey, this);
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



        private bool IsHaveSerial()
        {
            bool result = true;
            if (string.IsNullOrEmpty(Build.Serial))
            {
                var dialog = new AlertDialog.Builder(this);
                dialog.SetTitle(Resources.GetString(Resource.String.SerialError));
                dialog.SetMessage(Resources.GetString(Resource.String.SerialErrorDescription));
                dialog.SetNegativeButton("OK", (sender, args) =>
                {
                    Finish();
                });
                dialog.SetCancelable(false);
                dialog.Show();
                result = false;
            }
            return result;
        }
    }
}