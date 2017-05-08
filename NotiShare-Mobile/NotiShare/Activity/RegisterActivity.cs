using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace NotiShare.Activity
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
    public class RegisterActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.register_layout);
            // Create your application here
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}