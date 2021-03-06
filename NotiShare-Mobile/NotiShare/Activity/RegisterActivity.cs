﻿using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Interop;
using NotiShare.Helper;
using NotiShareModel.CrossHelper;
using NotiShareModel.DataTypes;
using NotiShareModel.HttpWorker;

namespace NotiShare.Activity
{
    [Activity(Label = "@string/Register", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegisterActivity : AppCompatActivity
    {
        private TextInputLayout emailLayout, passwordInputLayout, passwordRepeaTextInputLayout, userNameInputLayout;
        private TextInputEditText emailEditText, passwordEditText, passwrodRepeatEditText, userNameInputEditText, nameEditText, surnamEditText;
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
            userNameInputLayout = FindViewById<TextInputLayout>(Resource.Id.loginNameInput);
            

            progressBar = FindViewById<RelativeLayout>(Resource.Id.progressBar);
            mainLayout = FindViewById<RelativeLayout>(Resource.Id.homeLayout);

            emailEditText = FindViewById<TextInputEditText>(Resource.Id.emailTextEdit);
            passwordEditText = FindViewById<TextInputEditText>(Resource.Id.passwordEditText);
            passwrodRepeatEditText = FindViewById<TextInputEditText>(Resource.Id.repeatpasswordEditText);
            userNameInputEditText = FindViewById<TextInputEditText>(Resource.Id.loginEditText);
            nameEditText = FindViewById<TextInputEditText>(Resource.Id.userNameEditText);
            surnamEditText = FindViewById<TextInputEditText>(Resource.Id.surnameEditText);

            emailEditText.TextChanged += EmailEditTextOnTextChanged;
            passwordEditText.TextChanged += PasswordEditTextOnTextChanged;
            passwrodRepeatEditText.TextChanged += PasswrodRepeatEditTextOnTextChanged;
            // Create your application here
        }

        private void PasswrodRepeatEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ValidationHelper.DisableError(passwordRepeaTextInputLayout);
        }

        private void PasswordEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ValidationHelper.DisableError(passwordInputLayout);
        }

        private void EmailEditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            ValidationHelper.DisableError(emailLayout);
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


        [Export("onRegister")]
        public async void RegisterClick(View view)
        { 
            if (!CrossValidationHelper.CheckEmail(emailEditText.Text))
            {
                ValidationHelper.PutErrorMessage(emailLayout, Resources.GetString(Resource.String.EmailError));
                return;
            }
            if (!CrossValidationHelper.ValidatePasswordLenght(passwordEditText.Text))
            {
                ValidationHelper.PutErrorMessage(passwordInputLayout, Resources.GetString(Resource.String.PasswordLengthError));
                return;
            }
            if (!CrossValidationHelper.ValidatePasswords(passwordEditText.Text, passwrodRepeatEditText.Text))
            {
                ValidationHelper.PutErrorMessage(passwordRepeaTextInputLayout, Resources.GetString(Resource.String.PasswordsDoNotMatch));
                return;
            }
            progressBar.Visibility = ViewStates.Visible;
            mainLayout.Visibility = ViewStates.Gone;
            SupportActionBar.Hide();
            RegistrationObject registerObject = null;
            await Task.Run(() =>
            {
                registerObject = new RegistrationObject
                {
                    Email = emailEditText.Text,
                    PasswordHash = HashHelper.GetHashString(passwordEditText.Text),
                    Name = nameEditText.Text,
                    Surname = surnamEditText.Text,
                    UserName = userNameInputEditText.Text
                };
            });
            var result = await HttpWorker.Instance.RegisterUser(registerObject);
            if (result.Equals("Registered"))
            {
                Finish();   
            }
            else
            {
                progressBar.Visibility = ViewStates.Gone;
                mainLayout.Visibility = ViewStates.Visible;
                SupportActionBar.Show();
            }
            AppHelper.ShowToastText(this, result);

        }    
    }
}