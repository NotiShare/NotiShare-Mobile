using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using NotiShare.Services;

namespace NotiShare.BroadcastReceivers
{
    [BroadcastReceiver(Name = "com.fezz.notishare.BoolComplete")]
    public class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        { 
            var preference = PreferenceManager.GetDefaultSharedPreferences(context);
            if (preference.GetBoolean("notification", false))
            {
                var notificationService = new Intent(context, typeof(NotificationService));
                context.StartService(notificationService);
            }
            if (preference.GetBoolean("clipboard", false))
            {
                var clipboardService = new Intent(context, typeof(ClipboardService));
                context.StartService(clipboardService);
            }
            Log.Info("notishare", "boot complete");
        }
    }
}