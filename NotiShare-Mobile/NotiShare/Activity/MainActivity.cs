using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace NotiShare.Activity
{
    [Activity(Label = "NotiShare", Theme = "@style/Theme.AppCompat.Light.DarkActionBar",ScreenOrientation = ScreenOrientation.Portrait, MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }
    }
}

