using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using NotiShareModel.DataTypes;

namespace NotiShare.Helper
{
    internal static class ValidationHelper
    {

        internal static bool CheckEmail(string inputEmail)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            var match = regex.Match(inputEmail);
            return match.Success;
        }


        internal static bool ValidatePasswordLenght(string password)
        {
            return password.Length > 5 && password.Length < 17;
        }


        internal static bool ValidatePasswords(string originalPassword, string repeatedPassword)
        {
            return originalPassword.Equals(repeatedPassword);
        }


        internal static void PutErrorMessage(TextInputLayout layout, string text)
        {
            layout.ErrorEnabled = true;
            layout.Error = text;
        }


        internal static void DisableError(TextInputLayout layout)
        {
            if (layout.ErrorEnabled)
            {
                layout.ErrorEnabled = false;
            }
        }


        internal static LoginObject CanAuthorize(Context context)
        {
            LoginObject returnObject = null;
            var email = AppHelper.ReadString("loginName", string.Empty, context);
            var password = AppHelper.ReadString("password", string.Empty, context);
            if ((!string.IsNullOrEmpty(email)) || (!string.IsNullOrEmpty(password)))
            {
                returnObject = new LoginObject
                {
                    Email = email,
                    PasswordHash = password
                };
            }
            return returnObject;
        }
    }
}