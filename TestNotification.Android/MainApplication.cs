using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;

using Firebase;
using Firebase.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestNotification.Droid
{
    [Application]
    public class MainApplication : Android.App.Application
    {
        internal static readonly string CHANNEL_ID = "C4mobile_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        public static MainApplication MyApp { get; private set; }
        public const string SECURE_KEY = "Config";

        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            MyApp = this;
            var config = ReadSecureStorage();
            if (config != null)
            {
                var options = CreateOptions(config);
                Initialize(options, FirebaseApp.DefaultAppName);
                ConfigurationFirebaseMessaging();
            }
        }

        private Dictionary<string, string> Deserialize(string list)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(list);

        }
        private Dictionary<string, string> ReadSecureStorage()
        {
            var task = SecureStorage.GetAsync(SECURE_KEY);
            Task.WaitAll(task);
            if (!string.IsNullOrEmpty(task.Result))
            {
                return Deserialize(task.Result);
            }
            else
            {
                return null;
            }
        }
        public void Initialize(FirebaseOptions options, string name)
        {
            FirebaseApp.InitializeApp(this, options, name);

            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                CreateNotificationChannel();
            }
        }
        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        public void ConfigurationFirebaseMessaging()
        {
            FirebaseMessaging.Instance.AutoInitEnabled = true;
            FirebaseMessaging.Instance.SubscribeToTopic("all");
        }
        public async Task<bool> TryDeleteFirebaseAppAsync(string name)
        {
            var apps = FirebaseApp.GetApps(this);
            if (apps.Count != 0)
            {
                var app = apps.Where((i) => i.Name == name).FirstOrDefault();
                await FirebaseMessaging.Instance.DeleteToken();
                app.Delete();
                return true;
            }
            else
                return false;

        }
        public FirebaseOptions CreateOptions(Dictionary<string, string> config)
        {
            return new FirebaseOptions.Builder()
            .SetApiKey(config["API_KEY"])
            .SetApplicationId(config["GOOGLE_APP_ID"])
            .SetGcmSenderId(config["GCM_SENDER_ID"])
            .SetProjectId(config["PROJECT_ID"])
            .SetStorageBucket(config["STORAGE_BUCKET"])
            .Build();
        }
    }
}
