using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Essentials;
using Android.Gms.Common;
using Android.Content;
using System.Collections.Generic;
using Xamarin.Forms;
using System;
using Firebase.Messaging;
using System.Threading.Tasks;
using Android.Gms.Extensions;

namespace TestNotification.Droid
{
    [Activity(Label = "TestNotification", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        private bool IsUseConfig
        {
            get { return Preferences.Get(nameof(IsUseConfig), false); }
            set
            {
                Preferences.Set(nameof(IsUseConfig), value);
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            IsPlayServicesAvailable();
            MessagingCenter.Subscribe<MainPage, Dictionary<string, string>>(this, "Config", async (sender, config) =>
            {
                try
                {
                    SecureStorage.Remove(MainApplication.SECURE_KEY);
                    SaveToSecureStorage(config);
                    if (!IsUseConfig)
                    {
                        Application.OnCreate();
                        IsUseConfig = true;

                    }
                    else
                    {
                        await SetNextConfigAsync(config);
                        await GetToken();
                    }
                }
                catch (Exception ex)
                {

                }
            });

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
        }
        private string Serialize(Dictionary<string, string> valuePairs)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(valuePairs);
        }
        private void SaveToSecureStorage(Dictionary<string, string> valuePairs)
        {
            var list = Serialize(valuePairs);
            SecureStorage.SetAsync(MainApplication.SECURE_KEY, list);
        }
        private async Task SetNextConfigAsync(Dictionary<string, string> config)
        {
            var options = MainApplication.MyApp.CreateOptions(config);
            await MainApplication.MyApp.TryDeleteFirebaseAppAsync("[DEFAULT]");
            MainApplication.MyApp.Initialize(options, "[DEFAULT]");
            MainApplication.MyApp.ConfigurationFirebaseMessaging();
        }
        private async Task GetToken()
        {
            var token = await FirebaseMessaging.Instance.GetToken().AsAsync<IJavaObject>();
            token.ToString();

        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    _msgText = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    Finish();
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
