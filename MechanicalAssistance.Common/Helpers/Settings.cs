﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MechanicalAssistance.Common.Helpers
{
    public static class Settings
    {
        private const string _user = "user";
        private const string _token = "token";
        private const string _isLogin = "isLogin";
        private const string _service = "service";
        private const string _request = "request";
        private static readonly string _stringDefault = string.Empty;
        private static readonly bool _boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;


        public static string User
        {
            get => AppSettings.GetValueOrDefault(_user, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_user, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static bool IsLogin
        {
            get => AppSettings.GetValueOrDefault(_isLogin, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isLogin, value);
        }

        public static string Service
        {
            get => AppSettings.GetValueOrDefault(_service, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_service, value);
        }

        public static string Request
        {
            get => AppSettings.GetValueOrDefault(_request, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_request, value);
        }
    }
}
