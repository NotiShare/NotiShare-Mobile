using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Java.Interop;

namespace NotiShare.Activity
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.DarkActionBar",ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.login_screen);
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }


        [Export("onRegister")]
        public void RegisterClick(View view)
        {
            var intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
        }
    }
}

