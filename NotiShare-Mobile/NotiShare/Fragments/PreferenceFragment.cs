using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Preferences;
using Android.Provider;
using NotiShare.Services;

namespace NotiShare.Fragments
{
    public class PreferenceFragment : Android.Preferences.PreferenceFragment, ISharedPreferencesOnSharedPreferenceChangeListener, IDialogInterfaceOnCancelListener
    {
        private CheckBoxPreference notificationPreference;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AddPreferencesFromResource(Resource.Layout.preference_layout);
            PreferenceManager.GetDefaultSharedPreferences(Activity).RegisterOnSharedPreferenceChangeListener(this);
            notificationPreference = (CheckBoxPreference) FindPreference("notification");
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
        {
            Log.Info("notishare", "preference change");
            switch (key)
            {
                case "notification":
                    var notificationIntent = new Intent(Activity, typeof(NotificationService));
                    if (sharedPreferences.GetBoolean(key, false))
                    {
                        EnableService(notificationIntent);
                    }
                    else
                    {
                        Activity.StopService(notificationIntent);
                    }
                    break;
                case "clipboard":
                    var clipboardIntent = new Intent(Activity, typeof(ClipboardService));
                    if (sharedPreferences.GetBoolean(key, false))
                    {
                        Activity.StartService(clipboardIntent);
                    }
                    else
                    {
                        Activity.StopService(clipboardIntent);
                    }
                    break;
            }
        }



        private void EnableService(Intent currentIntent)
        {
            if (!Settings.Secure.GetString(Activity.ContentResolver, "enabled_notification_listeners")
                .Contains(Activity.PackageName))
            {
                var dialog = new AlertDialog.Builder(Activity);
                dialog.SetTitle(Resources.GetString(Resource.String.NotificationDialogTitle));
                dialog.SetMessage(Resources.GetString(Resource.String.NotificationDialogMessage));
                dialog.SetNegativeButton("OK", (sender, args) =>
                {
                    StartActivity(new Intent(Settings.ActionNotificationListenerSettings));
                    Activity.StartService(currentIntent);
                });
                dialog.SetCancelable(true);
                dialog.SetOnCancelListener(this);
                dialog.Show();      
            }
        }


        public void OnCancel(IDialogInterface dialog)
        {
            var preferenceManager = PreferenceManager.SharedPreferences;
            var editor = preferenceManager.Edit();
            editor.PutBoolean("notification", false);
            editor.Apply();
            notificationPreference.Checked = false;
        }
    }
}