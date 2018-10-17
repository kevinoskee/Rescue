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
                ///GetModel(emergency);
                //SendMessage(emergency.ToString());
               await DependencyService.Get<IGetLocation>().Location();
               // CrossLocalNotifications.Current.Show("title", "body");

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
                    };
                    db.AddMessage(Message);
                }

            }

        }

        public async void SendMessage(string emergency)
        {

            //await DependencyService.Get<IGetLocation>().Location();
            string defaultMsg = "Help me";
            MessageDatabase mdb = new MessageDatabase(dbPath);
            var msg = await mdb.GetMessageAsync(emergency);
            if (msg.MessageTemplate == null)
                msg.MessageTemplate = defaultMsg;
            ContactDatabase cdb = new ContactDatabase(dbPath);
            var list = await cdb.GetContactsAsync(emergency);
            if (list.Count() > 0)
            {
                foreach (Contact contact in list)
                {

                    DependencyService.Get<ISendSMS>().Send(contact.ContactNumber, msg.MessageTemplate);
                }
            }
            else
                DependencyService.Get<IToast>().Toasts("hasData", "failed");
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
     
       
    }
}
