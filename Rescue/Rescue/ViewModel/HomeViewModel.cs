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
            EmergencyTap= new Command(OnEmergencyTap);
            InitMessage();
            InitLocation();
        }
    
  
        public async void OnEmergencyTap(object emergency)
        {
            if (EmergencyMode == "Setup Mode")
            {
                await App.NavigateMasterDetail(new SetUpView(emergency.ToString()));
            }
            else
            {
                 CheckEmergency(emergency.ToString());
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
     
       
    }
}
