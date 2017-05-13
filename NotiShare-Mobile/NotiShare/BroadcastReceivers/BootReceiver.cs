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
using NotiShare.Helper;
using NotiShare.Services;
using NotiShareModel.HttpWorker;

namespace NotiShare.BroadcastReceivers
{
    [BroadcastReceiver(Name = "com.fezz.notishare.BoolComplete")]
    public class BootReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            Websockets.Droid.WebsocketConnection.Link();
            if (AppHelper.IsConnectedToNetwork())
            {
                var loginObject = ValidationHelper.CanAuthorize(Application.Context);
                if (loginObject != null)
                {
                    var result = await HttpWorker.Instance.Login(loginObject);
                    if (result.Equals("Welcome"))
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
        }
    }
}