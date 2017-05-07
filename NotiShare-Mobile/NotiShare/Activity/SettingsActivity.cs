using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using NotiShare.Fragments;

namespace NotiShare.Activity
{
    [Activity(Label = "@string/SettingsTitle",Theme = "@style/Theme.AppCompat.Light.DarkActionBar", ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true)]
    public class SettingsActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings_layout);
            var settingFragment = new PreferenceFragment();
            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.settings_fragment, settingFragment);
            transaction.Commit();
            // Create your application here
        }
    }
}