using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;

namespace TestNotification.Droid.Services
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class CustomFirebaseMessagingService : FirebaseMessagingService
    {

        public CustomFirebaseMessagingService()
        {
        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            Log.Debug("FMC_SERVICE", token);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            var notification = message.GetNotification();
        }
    }
}
