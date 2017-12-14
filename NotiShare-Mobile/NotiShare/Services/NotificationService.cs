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
                socket = new WebSocket("notificationSocket", 3031, AppHelper.ReadInt(PreferenceKeys.UserDeviceId, Application.Context, -1), AppHelper.ReadInt(PreferenceKeys.UserIdKey, Application.Context, -1), 1);
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
            Drawable smallIcon = null;
            Drawable bigIcon = null;
            if (preference.GetBoolean(PreferenceKeys.NotificationKey, false))
            {
                try
                {
                    Log.Info(DebugConstant, "post notification");
                    var pack = sbn.PackageName;
                    var bundle = sbn.Notification.Extras;
                    title = bundle.GetString(Notification.ExtraTitle);
                    text = bundle.GetString(Notification.ExtraText);



                    var sbnAppContext = CreatePackageContext(pack, PackageContextFlags.IgnoreSecurity);

                    var largeIcon = sbn.Notification.GetLargeIcon();
                    var toolbarIcon = sbn.Notification.SmallIcon;

                    await Task.Run(() =>
                    {

                       
                        bigIcon = largeIcon?.LoadDrawable(sbnAppContext);
                        smallIcon = toolbarIcon?.LoadDrawable(sbnAppContext);

                        var notificationObjet = new NotificationObject
                        {
                            ImageBase64 = GetImageString(smallIcon, bigIcon),
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

        private string GetImageString(Drawable smallDrawable, Drawable bigDrawable)
        {
            if (bigDrawable != null)
            {
                return GetStringFromDrawable(bigDrawable);
            }
            else
            {
                if (smallDrawable != null)
                {
                    return GetStringFromDrawable(smallDrawable);
                }
            }
            return string.Empty;

        }



        private string GetStringFromDrawable(Drawable drawable)
        {
            Bitmap bitmap = null;
            try
            {
                bitmap = ((BitmapDrawable)drawable).Bitmap;
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