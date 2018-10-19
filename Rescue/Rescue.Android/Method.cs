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
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rescue.Database;
using System.IO;
using Rescue.ViewModel;
[assembly: Dependency(typeof(Rescue.Droid.Method))]
namespace Rescue.Droid
{
    [Activity(Label = "Method", ParentActivity = typeof(MainActivity))]
    public class Method : IGetLocation, IToast, IHideKeyboard, ISendSMS
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Rescue.db3");
        static string prevLocation;
        static string currLocation;
        Toast toast = Toast.MakeText(Forms.Context, "", ToastLength.Short);

        public async Task Location()
        {
            var locationStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (locationStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                locationStatus = results[Permission.Location];
                if (locationStatus != PermissionStatus.Granted)
                {
                    DependencyService.Get<IToast>().Toasts("custom", "Permission Denied");
                    return;
                }
            }

            try
            {
                Geocoder geoCoder = new Geocoder();

                Toasts("custom", "Getting Location");

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;

                try
                {
                    var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(3));
                    var reversePosition = new Position(position.Latitude, position.Longitude);
                    var possibleAddresses = (await geoCoder.GetAddressesForPositionAsync(reversePosition)).FirstOrDefault();
                    Toasts("custom", "You're at : " + possibleAddresses);

                    LocationDatabase db = new LocationDatabase(dbPath);

                    var Location = new Location()
                    {
                        CurrentLocation = possibleAddresses
                    };
                    db.AddLocation(Location);
                    currLocation = possibleAddresses;
                    
                }
                catch (Exception e)
                {
                    Toasts("custom", "Cannot get current location");
                    currLocation = "Unknown";
                    LocationDatabase db = new LocationDatabase(dbPath);
                    var tempLocation = await db.GetPrevLocationAsync();
                    if (tempLocation != null)
                        prevLocation = tempLocation.CurrentLocation;
                    else
                        prevLocation = "Unknown";
                    return;
                }
            }
            catch(Exception e)
            {
                Toasts("permission", "Location");
                return;
            }
           
        }

        public async void Send(string emergency)
        {
            var smsStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Sms);
            if (smsStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Sms });
                smsStatus = results[Permission.Sms];
                if (smsStatus != PermissionStatus.Granted)
                {
                    DependencyService.Get<IToast>().Toasts("custom", "Permission Denied");
                    return;
                }
            }
            try
            {
                string fullname = "";
                string address = "";
                int age = 0;
                string bloodgroup = "";
                string medications = "";
                string allergies = "";
                string otherinfo = "";
                bool noLocData = true;
                string message1 = "Emergency Message:\n\n";
                string message2 = "Other information:\n\n";

                Geocoder geoCoder = new Geocoder();

                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
              
                ProfileDatabase profileDB = new ProfileDatabase(dbPath);
                var profile = await profileDB.GetProfileAsync();
                fullname = profile.FullName;
                address = profile.Address;
                age = ((DateTime.Now.DayOfYear < profile.BirthDate.DayOfYear) ? DateTime.Now.Year - profile.BirthDate.Year - 1 : DateTime.Now.Year - profile.BirthDate.Year);
                bloodgroup = profile.BloodGroup;
                medications = (profile.Medications ?? "");
                allergies = (profile.Allergies ?? "");
                otherinfo = (profile.OtherInfo ?? "");

                MessageDatabase messageDB = new MessageDatabase(dbPath);
                var messageTemp = await messageDB.GetMessageAsync(emergency);
                message1 += messageTemp.MessageTemplate;

                StringBuilder str1 = new StringBuilder();
                if (currLocation != "Unknown")
                    str1.Append("\nCurrent Location: " + currLocation);
                else if(currLocation == "Unknown" && prevLocation != null)
                {
                    str1.Append("\nCurrent Location: " + currLocation);
                    str1.Append("\nPrevious Location: " + prevLocation);
                }
                else
                {
                    str1.Append("\nCurrent Location: " + currLocation);
                    str1.Append("\nPrevious Location: " + prevLocation);
                    str1.Append("\nAddress: " + address);
                    noLocData = true;
                }
                str1.Append("\nName: " + fullname);
                message1 = string.Concat(message1, str1.ToString());

                StringBuilder str2 = new StringBuilder();
                if(!noLocData)
                    str2.Append("Address: " + address + "\n");
                str2.Append("Age: " + age.ToString());
                str2.Append("\nBlood Group: " + bloodgroup);
                if (medications != "")
                    str2.Append("\nMedications: " + medications);
                if (allergies != "")
                    str2.Append("\nAllergies: " + allergies);
                if (otherinfo != "")
                    str2.Append("\nOther Information: " + otherinfo);
                message2 = string.Concat(message2, str2.ToString());



                ContactDatabase contactDB = new ContactDatabase(dbPath);
                var list = await contactDB.GetContactsAsync(emergency);
                foreach (Contact contact in list)
                {
                    SmsManager.Default.SendTextMessage(contact.ContactNumber, null, message1, null, null);
                    SmsManager.Default.SendTextMessage(contact.ContactNumber, null, message2, null, null);
                    Toasts("custom", "SMS Sent");
                }
             
            }
            catch(Exception e)
            {
                Toasts("permission","send sms");
            }  
        
        }


        public void Toasts(string function, string status)
        {
            switch (function)
            {
               case "hasData":
                    if (status == "success")
                    {
                        toast.SetText("Data Available");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("No Data Yet. Please setup first.");
                        toast.Show();
                    }
                    break;

                case "hasEntry":
                    if (status == "success")
                    {
                        toast.SetText("Still has");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("No More");
                        toast.Show();
                    }
                    break;
                case "addProfile":
                    if (status == "success")
                    {
                        toast.SetText("Profile Created");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Please enter data");
                        toast.Show();
                    }
                    break;
                case "updateProfile":
                    if (status == "success")
                    {
                        toast.SetText("Profile Updated");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Please enter data");
                        toast.Show();
                    }
                    break;
                case "addContact":
                    if (status == "success")
                    {
                        toast.SetText("Contact Added");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Please enter data");
                        toast.Show();
                    }
                    break;
                case "deleteContact":
                    if (status == "success")
                    {
                        toast.SetText("Contact Deleted");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Something wrong");
                        toast.Show();
                    }
                    break;
                case "updateContact":
                    if (status == "success")
                    {
                        toast.SetText("Contact Updated");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Please enter data");
                        toast.Show();
                    }
                    break;
                case "updateMessage":
                    if (status == "success")
                    {
                        toast.SetText("Message Updated");
                        toast.Show();
                    }
                    else
                    {
                        toast.SetText("Default Message Template");
                        toast.Show();
                    }
                    break;
                case "permission":
                    toast.SetText("Grant " + status + " permission in the settings first.");
                    toast.Show();
                    break;
                case "emergency":
                    toast.SetText(status);
                    toast.Show();
                    break;
                case "custom":
                    toast.SetText(status);
                    toast.Show();
                    break;
            }
        }

        public void HideKeyboard()
        {
            var context = Forms.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
        public void Test()
        { 

            Random random = new Random();
            int randomNumber = random.Next(9999 - 1000) + 1000;
            var localNotification = new LocalNotification
            {
                Title = "Emergency Triggered",
                Body = "Sending emergency",
                Id = 0,
                NotifyTime = DateTime.Now,
                IconId = Resource.Drawable.ic_dialog_close_light
                
            };
            Intent newIntent = new Intent(Android.App.Application.Context, typeof(MainActivity));
             Android.Support.V4.App.TaskStackBuilder stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Android.App.Application.Context);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(newIntent);
            PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
     
            var builder = new Notification.Builder(Android.App.Application.Context)
                .SetContentTitle(localNotification.Title)
                .SetContentText(localNotification.Body)
                .SetSmallIcon(localNotification.IconId)
                .SetContentIntent(resultPendingIntent)
                .SetAutoCancel(true);
            var notificationManager = NotificationManager.FromContext(Android.App.Application.Context);
            notificationManager.Notify(randomNumber, builder.Build());



           


        }







    }
}