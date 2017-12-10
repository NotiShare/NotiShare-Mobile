using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Preferences;
using Android.Service.Notification;
using Android.Support.V4.Content;
using Android.Util;
using Newtonsoft.Json;
using NotiShare.Helper;
using NotiShareModel.DataTypes;
using NotiShare.Ws;

namespace NotiShare.Services
{
    [Service(Name = "com.fezz.notishare.NotificationService")]
    public class NotificationService : NotificationListenerService
    {

        private WebSocket socket;
        private const string DebugConstant = "notishare_notificationService";
        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(DebugConstant, "start");
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (socket == null)
            {
                socket = new WebSocket("notificationSocket", 3031, AppHelper.ReadString(PreferenceKeys.UserDeviceId, string.Empty, Application.Context), AppHelper.ReadString(PreferenceKeys.UserIdKey, string.Empty, Application.Context), 1);
                socket.Init();
            }
            else
            {
                if (!socket.IsConnected())
                {
                    socket.Init();
                }
            }
            return StartCommandResult.Sticky;
        }


        public override async void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);
            var preference = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            string title, text;
            Drawable icon = null;
            if (preference.GetBoolean(PreferenceKeys.NotificationKey, false))
            {
                try
                {
                    Log.Info(DebugConstant, "post notification");
                    var pack = sbn.PackageName;
                    var bundle = sbn.Notification.Extras;
                    title = bundle.GetString(Notification.ExtraTitle);
                    text = bundle.GetString(Notification.ExtraText);
                    var iconInt = bundle.GetInt(Notification.ExtraSmallIcon);
                    try
                    {
                        var iconByte = bundle.GetByteArray(Notification.ExtraLargeIconBig);
                        if (iconByte != null)
                        {
                            Log.Debug(DebugConstant, "Got big Icon");
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Debug(DebugConstant, e.StackTrace);
                    }


                var context = CreatePackageContext(pack, PackageContextFlags.IgnoreSecurity);
                    icon = ContextCompat.GetDrawable(context, iconInt);


                    await Task.Run(() =>
                    {

                        var notificationObjet = new NotificationObject
                        {
                            ImageBase64 = icon != null ? GetImageString(icon) : string.Empty,
                            NotificationText = !string.IsNullOrEmpty(text) ? text : "Empty text",
                            Title = !string.IsNullOrEmpty(title) ? title : "Empty title",
                            DatetimeCreation = DateTime.Now
                        }; // send to socket
                        var jsonString = JsonConvert.SerializeObject(notificationObjet);
                        socket.Send(jsonString);
                    });

                }
                catch (PackageManager.NameNotFoundException ex)
                {
                    Log.Error(DebugConstant, ex.StackTrace);
                }
                catch (Exception ex)
                {
                    Log.Error(DebugConstant, ex.StackTrace);
                }
            }
            
            else
            {
                Log.Info(DebugConstant, "service is disabled");
                socket.Close();
            }

        }

        private string GetImageString(Drawable drawable)
        {
            Bitmap bitmap = null;
            try
            {
                 bitmap = ((BitmapDrawable) drawable).Bitmap;
            }
            catch (InvalidCastException e)
            {
                Log.Error(DebugConstant, e.StackTrace);
                return string.Empty;
            }
            using (var stream = new MemoryStream())
            {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    return Convert.ToBase64String(stream.ToArray());
            }
            
        }
        

        public override void OnNotificationRemoved(StatusBarNotification sbn)
        {
            base.OnNotificationRemoved(sbn);
        }


        public override bool StopService(Intent name)
        {
            Log.Info(DebugConstant, "Stoped");
            socket.Close();
            return base.StopService(name);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Log.Info(DebugConstant, "stop");
        }
    }
}