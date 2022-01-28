using System;
using System.Collections.Generic;
using Firebase.CloudMessaging;
using Foundation;
using Plugin.FirebasePushNotification;
using UIKit;
using Xamarin.Forms;

namespace TestNotification.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        
        private bool _succeful;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            MessagingCenter.Subscribe<MainPage,Dictionary<string,string>>(this, "Config", (sender,mas) => UseConfig(options,mas));

            return base.FinishedLaunching(app, options);
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            Messaging.SharedInstance.Subscribe("all");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        // To receive notifications in foreground on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        private void UseConfig(NSDictionary options, Dictionary<string, string> config)
        {

            Firebase.Core.App.DefaultInstance?.Delete((e) => _succeful = e);
            var option = new Firebase.Core.Options(config["GOOGLE_APP_ID"], config["GCM_SENDER_ID"])
            {
                ClientId = config["CLIENT_ID"],
                ApiKey = config["API_KEY"],
                StorageBucket = config["STORAGE_BUCKET"],
                ProjectId = config["PROJECT_ID"],
                BundleId = config["BUNDLE_ID"],
            };
            Firebase.Core.App.Configure(option);
            FirebasePushNotificationManager.Initialize(options);

        }
    }
}
