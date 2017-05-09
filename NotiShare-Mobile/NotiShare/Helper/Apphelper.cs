using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
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

        internal static bool ReadBool(string key, Context context, bool defaulValue)
        {
            var preference = GetPreferences(context);
            return preference.GetBoolean(key, defaulValue);
        }

        private static ISharedPreferences GetPreferences(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context);
        }
    }
}
