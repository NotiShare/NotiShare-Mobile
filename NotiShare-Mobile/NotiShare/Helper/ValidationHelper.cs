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
            var user = AppHelper.ReadString(PreferenceKeys.LoginKey, string.Empty, context);
            var password = AppHelper.ReadString(PreferenceKeys.PasswordHash, string.Empty, context);
            if ((!string.IsNullOrEmpty(user)) || (!string.IsNullOrEmpty(password)))
            {
                returnObject = new LoginObject
                {
                    UserName = user,
                    PasswordHash = password
                };
            }
            return returnObject;
        }
    }
}