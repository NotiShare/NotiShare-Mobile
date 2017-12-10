using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Newtonsoft.Json;
using NotiShare.Helper;
using NotiShare.Ws;
using NotiShareModel.DataTypes;

namespace NotiShare.Services
{
    [Service(Name = "com.fezz.notishare.ClipboardService")]
    public class ClipboardService:Service, ClipboardManager.IOnPrimaryClipChangedListener
    {
        private ClipboardManager clipboardManager;
        private const string DebugTag = "notishare_clipboard";

        private WebSocket socket;
        public override IBinder OnBind(Intent intent)
        {
            return new Binder();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }


        public override void OnCreate()
        {
            base.OnCreate();
            clipboardManager = (ClipboardManager) GetSystemService(ClipboardService);
            clipboardManager.AddPrimaryClipChangedListener(this);
            socket = new WebSocket("clipboardSocket", 3032, AppHelper.ReadString(PreferenceKeys.UserDeviceId, string.Empty, Application.Context), AppHelper.ReadString(PreferenceKeys.UserIdKey, string.Empty, Application.Context), 1);
            socket.Init();
        }


        public async void OnPrimaryClipChanged()
        {
            Log.Info(DebugTag, "ClipboardTaken");
            var data = clipboardManager.PrimaryClip;
            if (clipboardManager.PrimaryClipDescription.HasMimeType(ClipDescription.MimetypeTextPlain))
            {
                var item = data.GetItemAt(0);
                var text = item.Text;
                await Task.Run(() =>
                {
                    if (!socket.IsConnected())
                    {
                        socket.Init();
                    }
                    var clipboardData = new ClipboardData
                    {
                        ClipboardStringData = text,
                        DataType = 1,
                        DatetimeCreation = DateTime.Now
                    };
                    socket.Send(JsonConvert.SerializeObject(clipboardData));
                });
            }
            
        }



        public override void OnDestroy()
        {
            base.OnDestroy();
            if (clipboardManager == null)
            {
                clipboardManager = (ClipboardManager) GetSystemService(ClipboardService);
            }
            clipboardManager.RemovePrimaryClipChangedListener(this);
            socket.Close();
            Log.Info(DebugTag, "disabled");
        }
    }
}