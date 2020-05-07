﻿using System.Globalization;
using MechanicalAssistance.Common.Interfaces;
using MechanicalAssistance.Prism.Resources;
using Xamarin.Forms;

namespace MechanicalAssistance.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }
        public static string ConnectionError => Resource.ConnectionError;
        public static string Accept => Resource.Accept_;
        public static string Error => Resource.Error;

        public static string VerificationEmail => Resource.VerificationEmail;
        public static string VerificationPassword => Resource.VerificationPassword;

        public static string VerificationUser => Resource.VerificationUser;

        public static string TitleLogin => Resource.TitleLogin;

        public static string PictureMessage => Resource.PictureMessage;
        public static string Cancel => Resource.Cancel;
        public static string Gallery => Resource.Gallery;
        public static string Camera => Resource.Camera;

        public static string Permissions => Resource.Permissions;
        public static string TitleRegister => Resource.TitleRegister;

        public static string Ok => Resource.Ok;

        public static string DocumentValidation => Resource.DocumentValidation;
        public static string FirstNameValidation => Resource.FirstNameValidation;
        public static string LastNameValidation => Resource.LastNameValidation;

        public static string AddressValidation => Resource.AddressValidation;
        public static string EmailValidation => Resource.EmailValidation;
        public static string PhoneValidation => Resource.PhoneValidation;
        public static string SizePassword => Resource.SizePassword;
        public static string PasswordValidation => Resource.PasswordValidation;

        //public static string passwordsMatch => Resource.passwordsMatch;

        public static string TitleRecoverPass => Resource.TitleRecoverPass;

        public static string ModifyMenu => Resource.ModifyMenu;
        public static string LogoutMenu => Resource.LogoutMenu;

        public static string UserUpdated => Resource.UserUpdated;

       // public static string btnChangePassword => Resource.btnChangePassword;
        public static string CurrentPasswordValidation => Resource.CurrentPasswordValidation;


    }
}