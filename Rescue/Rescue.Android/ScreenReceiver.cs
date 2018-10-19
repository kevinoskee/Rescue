using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Util;
using Plugin.Geolocator;
using System.Threading.Tasks;
using Rescue.Model;
using Xamarin.Forms.Maps;
using Android.Views.InputMethods;
using Android.Telephony;
using Xamarin.Android;
using Plugin.LocalNotifications;
using System.ServiceProcess;
using System.Threading;
using Rescue.Database;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
[assembly: Dependency(typeof(Rescue.Droid.ScreenReceiver))]
namespace Rescue.Droid
{
    public class ScreenReceiver : BroadcastReceiver
    {
        public int count;
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Rescue.db3");

        public override void OnReceive(Context context = null, Intent intent = null)
        {
            if (intent.Action == Intent.ActionScreenOff)
            {
                count++;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    count = 0;

                    return false;
                });
                if (count == 3)
                {
                    DependencyService.Get<IGetLocation>().Test();
                    DependencyService.Get<ISendSMS>().Send("Personal");
                    count = 0;
                }

            }
            else if (intent.Action == Intent.ActionScreenOn)
            {
                count++;
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    count = 0;

                    return false;
                });
                if(count == 3)
                {
                    DependencyService.Get<IGetLocation>().Test();
                    DependencyService.Get<ISendSMS>().Send("Personal");
                    count = 0;
                }
          
            }
        }
    }
}