using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Interop;

namespace NotiShare.Activity
{
    [Activity(Theme = "@style/Theme.AppCompat.Light.DarkActionBar")]
    public class RegisterActivity : AppCompatActivity
    {
        private TextInputLayout emailLayout, passwordInputLayout, passwordRepeaTextInputLayout;
        private TextInputEditText emailEditText, passwordEditText, passwrodRepeatEditText;
        private RelativeLayout progressBar, mainLayout;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SupportActionBar.SetDefaultDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.register_layout);

            emailLayout = FindViewById<TextInputLayout>(Resource.Id.emailInput);
            passwordInputLayout = FindViewById<TextInputLayout>(Resource.Id.passwordField);
            passwordRepeaTextInputLayout = FindViewById<TextInputLayout>(Resource.Id.repeatPasswordField);

            progressBar = FindViewById<RelativeLayout>(Resource.Id.progressBar);
            mainLayout = FindViewById<RelativeLayout>(Resource.Id.homeLayout);

            emailEditText = FindViewById<TextInputEditText>(Resource.Id.emailTextEdit);
            passwordEditText = FindViewById<TextInputEditText>(Resource.Id.passwordEditText);
            passwrodRepeatEditText = FindViewById<TextInputEditText>(Resource.Id.repeatpasswordEditText);

            emailEditText.TextChanged += EmailEditTextOnTextChanged;
            passwordEditText.TextChanged += PasswordEditTextOnTextChanged;
            passwrodRepeatEditText.TextChanged += PasswrodRepeatEditTextOnTextChanged;
            // Create your application here
        }

        private void PasswrodRepeatEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            DisableError(passwordRepeaTextInputLayout);
        }

        private void PasswordEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            DisableError(passwordInputLayout);
        }

        private void EmailEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            DisableError(emailLayout);
        }


        private void DisableError(TextInputLayout layout)
        {
            if (layout.ErrorEnabled)
            {
                layout.ErrorEnabled = false;
            }
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


        private bool CheckEmail(string inputEmail)
        {   
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(inputEmail);
            return match.Success;
        }


        [Export("onRegister")]
        public void RegisterClick(View view)
        {
            
        }
    }
}