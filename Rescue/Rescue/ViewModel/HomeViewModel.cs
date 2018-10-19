using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Rescue.View;
using Rescue.Model;
using System.Linq;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
using Rescue.Database;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Plugin.LocalNotifications;
using System.Threading;
namespace Rescue.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"Rescue.db3");
        public event PropertyChangedEventHandler PropertyChanged;
        public Action<string> ChangeButton;
        public bool hasContact;
       


        public ICommand Edit { protected set; get; }
        public ICommand Done { protected set; get; }
        public ICommand Return { protected set; get; }
        public ICommand EmergencyTap { protected set; get; }
        private string emergencyMode = "Emergency Mode";
        public string EmergencyMode
        {
            get { return emergencyMode; }
            set
            {
                emergencyMode = value;
                OnPropertyChanged();
            }
        }


        public HomeViewModel()
        {
            //Edit = new Command(OnEdit);
            //Done = new Command(OnDone);
            //Return = new Command(OnReturn);
            EmergencyTap= new Command(OnEmergencyTap);
            InitMessage();
            InitLocation();
  

        }
    
        //public void OnEdit()
        //{
        //    ChangeButton("edit");
        //    configStat = true;
        //}
        //public void OnDone()
        //{
        //    ChangeButton("done");
        //    configStat = false;
        //}
        //public void OnReturn()
        //{
        //    ChangeButton("return");
        //    configStat = false;
        //}
        public async void OnEmergencyTap(object emergency)
        {
            if (EmergencyMode == "Setup Mode")
            {
                await App.NavigateMasterDetail(new SetUpView(emergency.ToString()));
                //await App.NavigateMasterDetail(new SetUpView(emergency.ToString()));
            }
            else
            {

                 CheckEmergency(emergency.ToString());
                //{
                //    await InitLocation();
                //    await GetProfile();
                //}
                ///GetModel(emergency);
                //SendMessage(emergency.ToString());
                
               // await SetMessage(emergency.ToString());
                
                //await SendMessage(emergency.ToString());




            }
        }
        public async void InitMessage()
        {
            string[] emergencies = { "Police", "Medical", "Fire", "Personal" };
            foreach (string value in emergencies)
            {

                MessageDatabase db = new MessageDatabase(dbPath);
                var _db = await db.GetMessageAsync(value);
                if (_db == null)
                {
                    var Message = new Message()
                    {
                        EmergencyName = value,
                        MessageTemplate = "Help me"
                    };
                    db.AddMessage(Message);
                }

            }

        }

        public async void CheckEmergency(string emergency)
        {
            ContactDatabase db = new ContactDatabase(dbPath);
            var _db = await db.CountContact(emergency);
            if (_db > 0)
            {
                DependencyService.Get<ISendSMS>().Send(emergency);
            }
            else
                DependencyService.Get<IToast>().Toasts("hasData", "failed");
             
        }
        public async void InitLocation()
        {
           await DependencyService.Get<IGetLocation>().Location();
        }

        public async Task GetProfile()
        {
            //DependencyService.Get<IToast>().Toasts("custom", "Getting Profile");
            //ProfileDatabase profileDB = new ProfileDatabase(dbPath);
            //var profile = await profileDB.GetProfileAsync();
            //_name = profile.FirstName + " " + ((profile.MiddleName != null) ? profile.MiddleName.ElementAt(0).ToString() + ". " : "") + profile.LastName;
            //_address = profile.HouseNumber + " " + profile.Street + " St. Brgy. " + profile.Barangay + " " + profile.Town + ", " + profile.City;
            //_age = ((DateTime.Now.DayOfYear < profile.Birthdate.DayOfYear) ? DateTime.Now.Year - profile.Birthdate.Year - 1 : DateTime.Now.Year - profile.Birthdate.Year);
            //_bloodgroup = profile.BloodGroup;
            //_otherinfo = ((profile.OtherInfo != null) ? profile.OtherInfo : "");
            //DependencyService.Get<IToast>().Toasts("custom", "Got Profile!");
        }

        public async Task SetMessage(string emergency)
        {
            //DependencyService.Get<IToast>().Toasts("custom", "Setting Message");
            //string defaultMsg = "Help me\n\n";
            //MessageDatabase messageDB = new MessageDatabase(dbPath);
            //var msg = await messageDB.GetMessageAsync(emergency);
            //if (msg.MessageTemplate == null)
            //    message = defaultMsg;
            //else
            //    message = msg.MessageTemplate + "\n\n";
            //StringBuilder str = new StringBuilder();
            //str.Append((currLocation == "Unknown") ? "Previous Location: " + prevLocation + "\nCurrent Location:" + currLocation : "Current Location: " + currLocation);
            //str.Append("\n\nName: " + _name);
            //str.Append("\nAddress:" + _address);
            //str.Append("\nAge: " + _age);
            //str.Append("\nBlood Group: " + _bloodgroup);
            //str.Append((_otherinfo != "") ? "\nOther Information: " + _otherinfo : "");
            //message = string.Concat(message, str.ToString());
            //DependencyService.Get<IToast>().Toasts("custom", message);

        }

        public async void SendMessage(string emergency)
        {
           
               

          

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
     
       
    }
}
