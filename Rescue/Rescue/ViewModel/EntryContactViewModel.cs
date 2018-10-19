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
    public class EntryContactViewModel : INotifyPropertyChanged
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");
        public event PropertyChangedEventHandler PropertyChanged;

        private string emergencyName;
        public string EmergencyName
        {
            get { return emergencyName; }
            set
            {
                emergencyName = value;
            }
        }
        private string contactName;
        public string ContactName
        {
            get { return contactName; }
            set
            {
                contactName = value;
                NotifyPropertyChanged();
            }
        }
        private string contactNumber;
        public string ContactNumber
        {
            get { return contactNumber; }
            set
            {
                contactNumber = value;
                NotifyPropertyChanged();
            }
        }
        private int contactId;
        public int ContactId
        {
            get { return contactId; }
            set
            {
                contactId = value;
            }
        }
        public ICommand Add { protected set; get; }

        public EntryContactViewModel(string function, string emergency, int id = 0)
        {
            emergencyName = emergency;
            contactId = id;
            Add = new Command(OnAdd);
            if (function == "update")
                ShowData(emergency, id); 
        }

        public async void ShowData(string emergency, int id)
        {
            ContactDatabase db = new ContactDatabase(dbPath);
            var Contact = await db.GetContactAsync(emergency, id);
            EmergencyName = Contact.EmergencyName;
            ContactName = Contact.ContactName;
            ContactNumber = Contact.ContactNumber;
        }

        public async void OnAdd(object function)
        {
            ContactDatabase db = new ContactDatabase(dbPath);
            if (CheckFields())
            {
                switch (function)
                {
                    case "Add":
                        var Contact = new Contact()
                        {
                            EmergencyName = emergencyName,
                            ContactName = contactName,
                            ContactNumber = contactNumber
                        };
                        DependencyService.Get<IToast>().Toasts("addContact", db.AddContact(Contact));
                        ContactName = "";
                        ContactNumber = "";
                        MessagingCenter.Send<App>((App)Application.Current, "OnContactAdded");
                        break;
                    case "Update":
                        var UpdateContact = await db.GetContactAsync(emergencyName, contactId);
                        UpdateContact.ContactName = contactName;
                        UpdateContact.ContactNumber = contactNumber;
                        DependencyService.Get<IToast>().Toasts("updateContact", db.UpdateContact(UpdateContact));
                        MessagingCenter.Send<App>((App)Application.Current, "OnContactUpdated");
                        await PopupNavigation.Instance.PopAsync(true);
                        break;
                }

            }
            else
                DependencyService.Get<IToast>().Toasts("addContact", "failed");

        }

        bool CheckFields()
        {

            if (contactName != null && contactNumber != null)
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