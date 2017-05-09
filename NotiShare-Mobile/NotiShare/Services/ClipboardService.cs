using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace NotiShare.Services
{
    [Service(Name = "com.fezz.notishare.ClipboardService")]
    public class ClipboardService:Service, ClipboardManager.IOnPrimaryClipChangedListener
    {
        private ClipboardManager clipboardManager;
        private const string DebugTag = "notishare_clipboard";
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
        }

        public void OnPrimaryClipChanged()
        {
            Log.Info(DebugTag, "ClipboardTaken");
            var data = clipboardManager.PrimaryClip;
            if (clipboardManager.PrimaryClipDescription.HasMimeType(Android.Content.ClipDescription.MimetypeTextPlain))
            {
                var item = data.GetItemAt(0);
                var text = item.Text;
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
            Log.Info(DebugTag, "disabled");
        }
    }
}