using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Service.Notification;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace NotiShare.Services
{
    [Service(Name = "com.fezz.notishare.NotificationService")]
    public class NotificationService : NotificationListenerService
    {

        private const string DebugConstant = "notishare_notificationService";
        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(DebugConstant, "start");
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }


        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);
            var preference = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            if (preference.GetBoolean("notification", false))
            {
                Log.Info(DebugConstant, "post notification");
            }
            else
            {
                Log.Info(DebugConstant, "service is disabled");
            }

        }


        

        public override void OnNotificationRemoved(StatusBarNotification sbn)
        {
            base.OnNotificationRemoved(sbn);
        }


        public override bool StopService(Intent name)
        {
            Log.Info(DebugConstant, "Stoped");
            return base.StopService(name);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Info(DebugConstant, "stop");
        }
    }
}