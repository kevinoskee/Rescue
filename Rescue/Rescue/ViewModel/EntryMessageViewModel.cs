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
using Rescue.Database;
using SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using System.Linq;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;


namespace Rescue.ViewModel
{
    public class EntryMessageViewModel : INotifyPropertyChanged
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");
        public event PropertyChangedEventHandler PropertyChanged;
        //private Contact contact;
        //public Contact Contact
        //{
        //    get { return contact; }
        //    set
        //    {
        //        contact = value;
        //        NotifyPropertyChanged("Contact");
        //    }
        //}
        private string emergencyName;
        public string EmergencyName
        {
            get { return emergencyName; }
            set
            {
                emergencyName = value;
            }
        }
        private string messageTemplate;
        public string MessageTemplate
        {
            get { return messageTemplate; }
            set
            {
                messageTemplate = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand Save { protected set; get; }

        public EntryMessageViewModel(string emergency)
        {
            emergencyName = emergency;
            Save = new Command(OnSave);
            ShowData(emergency);
            // ShowContact("new", emergencyName);
            //contact.EmergencyName = emergency;
            //ShowData(emergency);
        }

        public async void ShowData(string emergency)
        {
            MessageDatabase db = new MessageDatabase(dbPath);
            var Message = await db.GetMessageAsync(emergency);
            EmergencyName = Message.EmergencyName;
            MessageTemplate = Message.MessageTemplate;

        }

        public async void OnSave()
        {
            
            MessageDatabase db = new MessageDatabase(dbPath);
            if (CheckFields())
            {
                
                        var SaveMessage = await db.GetMessageAsync(emergencyName);
                        SaveMessage.MessageTemplate = messageTemplate;
                        DependencyService.Get<IToast>().Toasts("updateMessage", db.UpdateMessage(SaveMessage));
                       
                       
                
            }
            else
                DependencyService.Get<IToast>().Toasts("updateMessage", "failed");
            await PopupNavigation.Instance.PopAsync(true);
        }

        bool CheckFields()
        {

            if (messageTemplate != null)
                return true;
            else

                return false;

        }

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

