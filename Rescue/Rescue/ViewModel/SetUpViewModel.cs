﻿using System;
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
    public class SetUpViewModel : INotifyPropertyChanged
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
        private string contactId;
        public string ContactId
        {
            get { return contactId; }
            set
            {
                contactId = value;
            }
        }

        public SetUpViewModel(string emergency)
        {
            emergencyName = emergency;
           // Add = new Command(OnAdd);
           // ShowContact("new", emergencyName);
            //contact.EmergencyName = emergency;
            //ShowData(emergency);
        }

        public void ShowData(string emergency)
        {
          
            //var _db = App.EmergencyDatabase.GetEmergency(emergency);
            //EmergencyImage = emergency.ToLower() + ".png";
            //EmergencyName = _db.EmergencyName;
            //ContactName = _db.ContactName;
            //ContactNumber = _db.ContactNumber;
            //MessageTemplate = _db.MessageTemplate;
        }
       
        // //public void OnAdd()
        //{
        //    if (CheckFields())
        //    {
        //        ContactDatabase db = new ContactDatabase(dbPath);
        //        var Contact = new Contact()
        //        {
        //            EmergencyName = emergencyName,
        //            ContactName = contactName,
        //            ContactNumber = contactNumber
        //        };
        //        //   DependencyService.Get<IToast>().Toasts("custom",emergency);
        //        DependencyService.Get<IToast>().Toasts("addContact", db.AddContact(Contact));
        //        ContactName = "";
        //        ContactNumber = "";
        //    }
        //    else
        //        DependencyService.Get<IToast>().Toasts("addContact", "failed");

        //}
        //bool CheckFields()
        //{

        //    if (contactName != null && contactNumber != null)
        //        return true;
        //    else

        //        return false;

        //}

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

