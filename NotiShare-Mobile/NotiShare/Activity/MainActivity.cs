using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Interop;
using NotiShare.Helper;
using NotiShareModel.DataTypes;
using NotiShareModel.HttpWorker;

namespace NotiShare.Activity
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.DarkActionBar",ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private TextInputLayout emailInputLayout, passwordInputLayout;
        private TextInputEditText emailText, passwordText;
        private RelativeLayout progressBar, mainLayout;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Websockets.Droid.WebsocketConnection.Link();
            SetContentView(Resource.Layout.login_screen);

            emailInputLayout = FindViewById<TextInputLayout>(Resource.Id.emailField);
            passwordInputLayout = FindViewById<TextInputLayout>(Resource.Id.passwordField);

            emailText = FindViewById<TextInputEditText>(Resource.Id.emailEditText);
            passwordText = FindViewById<TextInputEditText>(Resource.Id.passwordEditText);

            progressBar = FindViewById<RelativeLayout>(Resource.Id.progressBar);
            mainLayout = FindViewById<RelativeLayout>(Resource.Id.homeLayout);
            

            emailText.TextChanged += EmailTextOnTextChanged;
            passwordText.TextChanged += PasswordTextOnTextChanged;
            // Set our view from the "main" layout resource
            // SetContentView (Resource.Layout.Main);
        }


        public override async void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            var loginObject = ValidationHelper.CanAuthorize(this);
            if (loginObject != null)
            {
                var result = await HttpWorker.Instance.Login(loginObject);
                if (result.Equals("Welcome"))
                {
                    var intent = new Intent(this, typeof(AppActivity));
                    Finish();
                    StartActivity(intent);
                }
                else
                {
                    AppHelper.ShowToastText(this, result);
                    progressBar.Visibility = ViewStates.Gone;
                    mainLayout.Visibility = ViewStates.Visible;
                }
            }
            else
            {
                progressBar.Visibility = ViewStates.Gone;
                    mainLayout.Visibility = ViewStates.Visible;
            }
        }

        private void PasswordTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ValidationHelper.DisableError(passwordInputLayout);
        }

        private void EmailTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ValidationHelper.DisableError(emailInputLayout);
        }


        [Export("onRegister")]
        public void RegisterClick(View view)
        {
            var intent = new Intent(this, typeof(RegisterActivity));
            StartActivity(intent);
        }

        [Export("onLogin")]
        public async void LoginClick(View view)
        {
            
            if (!ValidationHelper.CheckEmail(emailText.Text))
            {
                ValidationHelper.PutErrorMessage(emailInputLayout, Resources.GetString(Resource.String.EmailError));
                return;
            }
            if (!ValidationHelper.ValidatePasswordLenght(passwordText.Text))
            {
                ValidationHelper.PutErrorMessage(passwordInputLayout, Resources.GetString(Resource.String.PasswordLengthError));
                return;
            }
            progressBar.Visibility = ViewStates.Visible;
            mainLayout.Visibility = ViewStates.Gone;
            var loginObject = new LoginObject
            {
                Email = emailText.Text,
                PasswordHash = HashHelper.GetHashString(passwordText.Text)
            };
            var result = await HttpWorker.Instance.Login(loginObject);
            if (result.Equals("Welcome"))
            {
                AppHelper.WriteString("loginName", emailText.Text, this);
                AppHelper.WriteString("password", loginObject.PasswordHash, this);
                var intent = new Intent(this, typeof(AppActivity));
                Finish();
                StartActivity(intent);
            }
            else
            {
                AppHelper.ShowToastText(this, result);
                progressBar.Visibility = ViewStates.Gone;
                mainLayout.Visibility = ViewStates.Visible;
            }
        }


        
    }
}

