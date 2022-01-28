using System.Collections.Generic;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;

namespace TestNotification
{
    public partial class MainPage : ContentPage
    {
        //Android 

        private Dictionary<string, string> config_Android_client_1 = new Dictionary<string, string>()
        {
            {"API_KEY","AIzaSyDMA8wPsGlnPE-0AGeAlCtMI--1QAV1hyU" },
            {"GCM_SENDER_ID","830577561928" },
            {"PROJECT_ID","notificationsample-56056" },
            {"STORAGE_BUCKET","notificationsample-56056.appspot.com" },
            {"GOOGLE_APP_ID","1:830577561928:android:bc1d02fb2bc2cd599c637d" },
        };

        private Dictionary<string, string> config_Android_client_2 = new Dictionary<string, string>()
        {
            {"API_KEY","AIzaSyDeaVPNJlLvtopf2yDeuH83l_EigdcpQlY" },
            {"GCM_SENDER_ID","523324461865" },
            {"PROJECT_ID","test-5y6056" },
            {"STORAGE_BUCKET","test-5y6056.appspot.com" },
            {"GOOGLE_APP_ID","1:523324461865:android:59e0390bd964cc95399d38" },
        };

        private Dictionary<string, string> config_Android_client_3 = new Dictionary<string, string>()
        {
            {"API_KEY","AIzaSyAEptmCPZojM0xFEvEeDV5Tuo3uzVFq3dg" },
            {"GCM_SENDER_ID","776796155193" },
            {"PROJECT_ID","mynextproject-a5e7e" },
            {"STORAGE_BUCKET","mynextproject-a5e7e.appspot.com" },
            {"GOOGLE_APP_ID","1:776796155193:ios:380ebf5ec475868869cce8" },
        };


        //iOS

        private Dictionary<string, string> config_iOS_client_1 = new Dictionary<string, string>()
        {
            {"API_KEY","AIzaSyDeHtpS-BOh5tKXDbkSbkChjaKwRKiTCfI" },
            {"GCM_SENDER_ID","830577561928" },
            {"BUNDLE_ID","com.test1.TestNotification" },
            {"PROJECT_ID","notificationsample-56056" },
            {"STORAGE_BUCKET","notificationsample-56056.appspot.com" },
            {"GOOGLE_APP_ID","1:830577561928:ios:3c6e26616b2ca0749c637d" },
            {"CLIENT_ID","830577561928-9tva61lk5eib8aaihi5c1ip2d9oinv7d.apps.googleusercontent.com" }
        };
        private Dictionary<string, string> config_iOS_client_2 = new Dictionary<string, string>()
        {
            {"API_KEY","AIzaSyAraBUIDxCnt5WOXF3NwB9RPvTAijMHLsE" },
            {"GCM_SENDER_ID","523324461865" },
            {"BUNDLE_ID","com.test1.TestNotification" },
            {"PROJECT_ID","test-5y6056" },
            {"STORAGE_BUCKET","test-5y6056.appspot.com" },
            {"GOOGLE_APP_ID","1:523324461865:ios:6c4077e7aad359e5399d38" },
            {"CLIENT_ID","523324461865-nbr4gntv17ukpmeqc52a8gsa4ldvncph.apps.googleusercontent.com" }
        };

        public MainPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
            }
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            DisplayAlert("Notification", $"Data: {e.Data["myData"]}", "OK");
        }

        void Button_Clicked(object sender, System.EventArgs e)
        {
            if(Device.RuntimePlatform == Device.iOS)
                MessagingCenter.Send(this, "Config", config_iOS_client_1);
            else if(Device.RuntimePlatform == Device.Android)
                MessagingCenter.Send(this, "Config", config_Android_client_1);
        }

        void Button_Clicked_1(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
                MessagingCenter.Send(this, "Config", config_iOS_client_2);
            else if (Device.RuntimePlatform == Device.Android)
                MessagingCenter.Send(this, "Config", config_Android_client_2);
        }

        void Button_Clicked_3(object sender, System.EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
                MessagingCenter.Send(this, "Config", config_Android_client_3);
        }
    }
}
