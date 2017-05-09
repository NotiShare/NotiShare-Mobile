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


        internal static void PutErrorMessage(TextInputLayout layout, int resourceString)
        {
            layout.ErrorEnabled = true;
            layout.Error = Resources.System.GetString(resourceString);
        }


        internal static void DisableError(TextInputLayout layout)
        {
            if (layout.ErrorEnabled)
            {
                layout.ErrorEnabled = false;
            }
        }
    }
}