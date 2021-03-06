﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace NotiShare.Helper
{
    internal static class AppHelper
    {

        

        internal static void ShowToastText(Context context, string text)
        {
            Toast.MakeText(context, text, ToastLength.Short).Show();
        }

        internal static void WriteString(string key, string value, Context appContext)
        {
            ISharedPreferences preferences = GetPreferences(appContext);
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutString(key, value);
            editor.Apply();
        }


        internal static string ReadString(string key, string defaultValue, Context appContext)
        {
            ISharedPreferences preferences = GetPreferences(appContext);
            return preferences.GetString(key, defaultValue);
        }


        internal static void ClearValue(string key, Context context)
        {
            var preferences = GetPreferences(context);
            var editor = preferences.Edit();
            editor.Remove(key);
            editor.Apply();
        }

        internal static void WriteBool(string key, bool value, Context context)
        {
            var preference = GetPreferences(context);
            var editor = preference.Edit();
            editor.PutBoolean(key, value);
            editor.Apply();
        }


        internal static void WriteInt(string key, int value, Context context)
        {
            var preference = GetPreferences(context);
            var editor = preference.Edit();
            editor.PutInt(key, value);
            editor.Apply();
        }


        internal static int ReadInt(string key, Context context, int defaultValue)
        {
            var preference = GetPreferences(context);
            return preference.GetInt(key, defaultValue);
        }

        internal static bool ReadBool(string key, Context context, bool defaulValue)
        {
            var preference = GetPreferences(context);
            return preference.GetBoolean(key, defaulValue);
        }

        private static ISharedPreferences GetPreferences(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context);
        }


        internal static bool IsConnectedToNetwork()
        {
            var connectivityManaget =
                (ConnectivityManager) Application.Context.GetSystemService(Context.ConnectivityService);
            var info = connectivityManaget.ActiveNetworkInfo;
            return info != null && info.IsConnected;
        }


        internal static bool IsServiceEnable(string name, Context context)
        {
            var activityManeger = (ActivityManager) context.GetSystemService(Context.ActivityService);
            foreach (var service in activityManeger.GetRunningServices(int.MaxValue))
            {
                if (name.Equals(service.Service.ClassName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
