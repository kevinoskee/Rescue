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
    public class EntryProfileViewModel : INotifyPropertyChanged
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");
        public event PropertyChangedEventHandler PropertyChanged;

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set
            {
                fullName = value;
                NotifyPropertyChanged();
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                NotifyPropertyChanged();
            }
        }
        private DateTime birthdate = DateTime.Parse((Convert.ToInt32(DateTime.Now.Year) - 1).ToString() + '/' + DateTime.Now.Month + '/' + DateTime.Now.Day);
        public DateTime BirthDate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
                NotifyPropertyChanged();
            }
        }
        private string bloodgroup;
        public string BloodGroup
        {
            get { return bloodgroup; }
            set
            {
                bloodgroup = value;
                NotifyPropertyChanged();
            }
        }
        private string medications;
        public string Medications
        {
            get { return medications; }
            set
            {
                medications = value;
                NotifyPropertyChanged();
            }
        }
        private string allergies;
        public string Allergies
        {
            get { return allergies; }
            set
            {
                allergies = value;
                NotifyPropertyChanged();
            }
        }
        private string otherinfo;
        public string OtherInfo
        {
            get { return otherinfo; }
            set
            {
                otherinfo = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand Add { protected set; get; }

        public EntryProfileViewModel(string function)
        {
           
            Add = new Command(OnAdd);
            if (function == "update")
                ShowData();
            // ShowContact("new", emergencyName);
            //contact.EmergencyName = emergency;
            //ShowData(emergency);
        }

        public async void ShowData()
        {
            ProfileDatabase db = new ProfileDatabase(dbPath);
            var _db = await db.GetProfileAsync();
            FullName = _db.FullName;
            Address = _db.Address;
            BirthDate = _db.BirthDate;
            BloodGroup = _db.BloodGroup;
            Medications = _db.Medications;
            Allergies = _db.Allergies;
            OtherInfo = _db.OtherInfo;
        }

        public async void OnAdd(object function)
        {
            ProfileDatabase db = new ProfileDatabase(dbPath);
            if (CheckFields())
            {
                switch (function)
                {
                    case "Add":
                        var Profile = new Profile()
                        {
                            FullName = fullName,
                            Address = address,
                            BirthDate = birthdate,
                            BloodGroup = bloodgroup,
                            Medications = medications,
                            Allergies = allergies,
                            OtherInfo = otherinfo
                        };
                        DependencyService.Get<IToast>().Toasts("addProfile", db.AddProfile(Profile));
                        await PopupNavigation.Instance.PopAsync(true);
                        break;
                    case "Update":
                        var UpdateProfile = await db.GetProfileAsync();
                        UpdateProfile.FullName = fullName;
                        UpdateProfile.Address = address;
                        UpdateProfile.BirthDate = birthdate;
                        UpdateProfile.BloodGroup = bloodgroup;
                        UpdateProfile.Medications = medications;
                        UpdateProfile.Allergies = allergies;
                        UpdateProfile.OtherInfo = otherinfo;
                        DependencyService.Get<IToast>().Toasts("updateProfile", db.UpdateProfile(UpdateProfile));
                        await PopupNavigation.Instance.PopAsync(true);
                        break;
                }

            }
            else
                DependencyService.Get<IToast>().Toasts("addProfile", "failed");
          
        }
        public bool CheckFields()
        {
            if (fullName != null && address != null && birthdate != null && bloodgroup != null)
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
