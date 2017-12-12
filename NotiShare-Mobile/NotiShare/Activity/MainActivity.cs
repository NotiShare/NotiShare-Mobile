using System;
using System.Threading.Tasks;
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
using NotiShareModel.CrossHelper;

namespace NotiShare.Activity
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait, Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private TextInputLayout loginInputLayout, passwordInputLayout;
        private TextInputEditText loginText, passwordText;
        private RelativeLayout progressBar, mainLayout;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
           
            SetContentView(Resource.Layout.login_layout);

            loginInputLayout = FindViewById<TextInputLayout>(Resource.Id.loginField);
            passwordInputLayout = FindViewById<TextInputLayout>(Resource.Id.passwordField);

            loginText = FindViewById<TextInputEditText>(Resource.Id.loginEditText);
            passwordText = FindViewById<TextInputEditText>(Resource.Id.passwordEditText);

            progressBar = FindViewById<RelativeLayout>(Resource.Id.progressBar);
            mainLayout = FindViewById<RelativeLayout>(Resource.Id.homeLayout);
            

            loginText.TextChanged += EmailTextOnTextChanged;
            passwordText.TextChanged += PasswordTextOnTextChanged;

            Websockets.Droid.WebsocketConnection.Link();
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
                if (result.Message.Equals("Welcome"))
                {
                    var intent = new Intent(this, typeof(AppActivity));
                    Finish();
                    StartActivity(intent);
                }
                else
                {
                    AppHelper.ShowToastText(this, result.Message);
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
            ValidationHelper.DisableError(loginInputLayout);
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
            if (!CrossValidationHelper.ValidatePasswordLenght(passwordText.Text))
            {
                ValidationHelper.PutErrorMessage(passwordInputLayout, Resources.GetString(Resource.String.PasswordLengthError));
                return;
            }
            progressBar.Visibility = ViewStates.Visible;
            mainLayout.Visibility = ViewStates.Gone;
            LoginObject loginObject = null;
            await Task.Run(() =>
            {
                loginObject = new LoginObject
                {
                    UserName = loginText.Text,
                    PasswordHash = HashHelper.GetHashString(passwordText.Text)
                };
            });
            var result = await HttpWorker.Instance.Login(loginObject);
            if (result.Message.Equals("Welcome"))
            {
                AppHelper.WriteString(PreferenceKeys.LoginKey, loginText.Text, this);
                AppHelper.WriteString(PreferenceKeys.PasswordHash, loginObject.PasswordHash, this);
                AppHelper.WriteInt(PreferenceKeys.UserIdKey, result.UserId, this);
                var intent = new Intent(this, typeof(AppActivity));
                Finish();
                StartActivity(intent);
            }
            else
            {
                AppHelper.ShowToastText(this, result.Message);
                progressBar.Visibility = ViewStates.Gone;
                mainLayout.Visibility = ViewStates.Visible;
            }
        }


        
    }
}

