using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Plugin.Permissions;
using Plugin.Geolocator;
using Android.Locations;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.CurrentActivity;
using Android;
namespace Rescue.Droid
{
    [Activity(Label = "Rescue", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        int powercount = 0;
        Intent intent;
        private PowerButtonService powerButtonService;
        Context ctx;
        public Context GetCtx()
        {
            return ctx;
        }

        protected async override void OnCreate(Bundle bundle)
        {
            await TryToGetPermissions();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            base.OnCreate(bundle);
            //var intent = new Intent(ApplicationContext, typeof(PowerButtonService));
            //var source = PendingIntent.GetBroadcast(ApplicationContext, 0, intent, 0);

            Rg.Plugins.Popup.Popup.Init(this, bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App());

            LocationManager locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);



            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                 ShowGPSDisabledAlertToUser();
            }
            IntentFilter filter = new IntentFilter(Intent.ActionScreenOn);
            filter.AddAction(Intent.ActionScreenOff);
            filter.AddAction(Intent.ActionUserPresent);
            ScreenReceiver myReceiver = new ScreenReceiver();
            RegisterReceiver(myReceiver, filter);
        }

        public void ShowGPSDisabledAlertToUser()
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("LOCATION SERVICE");
            alert.SetMessage("GPS is disabled in your device. Would you like to enable it?");
            alert.SetButton("Sure", (c, ev) =>
            {
                Intent i = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                StartActivity(i);
            });
            alert.SetButton2("No", (c, ev) =>
            {
                var activity = (Activity)Forms.Context;
                activity.FinishAffinity();
            });
            alert.Show();
        }

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }


        }

        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.SendSms,
                            Manifest.Permission.AccessNotificationPolicy,
                            Manifest.Permission.AccessLocationExtraCommands,
                            Manifest.Permission.AccessMockLocation,
                            Manifest.Permission.AccessNetworkState,
                            Manifest.Permission.AccessWifiState,
                            Manifest.Permission.Internet,
                            Manifest.Permission.Camera

             };

        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] != (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Permissions denied. Set in settings.", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {

            }
          
        }

    }
}

