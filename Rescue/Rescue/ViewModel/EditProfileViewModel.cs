using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rescue.Model;
using Rescue.View;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Rescue.Database;
namespace Rescue.ViewModel
{
    public class EditProfileViewModel
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"Rescue.db3");
        public event PropertyChangedEventHandler PropertyChanged;
        private int profileId;
        public int ProfileId
        {
            get { return profileId; }
            set
            {
                profileId = value;
            }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                NotifyPropertyChanged();
            }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
                NotifyPropertyChanged();
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
               lastName = value;
                NotifyPropertyChanged();
            }
        }
        private string houseNumber;
        public string HouseNumber
        {
            get { return houseNumber; }
            set
            {
                houseNumber = value;
                NotifyPropertyChanged();
            }
        }
        private string street;
        public string Street
        {
            get { return street; }
            set
            {
                street = value;
                NotifyPropertyChanged();
            }
        }
        private string barangay;
        public string Barangay
        {
            get { return barangay; }
            set
            {
                barangay = value;
                NotifyPropertyChanged();
            }
        }
        private string town;
        public string Town
        {
            get { return town; }
            set
            {
               town = value;
                NotifyPropertyChanged();
            }
        }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                NotifyPropertyChanged();
            }
        }
        private DateTime birthDate = DateTime.Parse((Convert.ToInt32(DateTime.Now.Year) - 1).ToString() + '/' + DateTime.Now.Month + '/' + DateTime.Now.Day);
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                NotifyPropertyChanged();
            }
        }
        private string bloodGroup;
        public string BloodGroup
        {
            get { return bloodGroup; }
            set
            {
                bloodGroup = value;
                NotifyPropertyChanged();
            }
        }
        private string otherInfo;
        public string OtherInfo
        {
            get { return otherInfo; }
            set
            {
                otherInfo = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand Create{ protected set; get; }
        public ICommand Save { protected set; get; }
     
        public Action<string,string> ShowAlert;

        public EditProfileViewModel(string function)
        {
            Create = new Command(OnCreate);
            Save = new Command(OnSave);
            if(function == "edit")
                ShowData();
        }
        public async void ShowData()
        {
            
            ProfileDatabase db = new ProfileDatabase(dbPath);
            var Profile = await db.GetProfileAsync();
            profileId = Profile.ProfileId;
            FirstName = Profile.FirstName;
            MiddleName = Profile.MiddleName;
            LastName = Profile.LastName;
            HouseNumber = Profile.HouseNumber;
            Street = Profile.Street;
            Barangay = Profile.Barangay;
            Town = Profile.Town;
            City = Profile.City;
            BirthDate = Profile.Birthdate;
            BloodGroup = Profile.BloodGroup;
            OtherInfo = Profile.OtherInfo;
        }
        public void OnCreate()
        {
            if (CheckFields())
            {
                var Profile = new Profile()
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    HouseNumber = houseNumber,
                    Street = street,
                    Barangay = barangay,
                    Town = town,
                    City = city,
                    Birthdate = birthDate,
                    BloodGroup = bloodGroup,
                    OtherInfo = otherInfo
                };
                ProfileDatabase db = new ProfileDatabase(dbPath);
                DependencyService.Get<IToast>().Toasts("createProfile", db.AddProfile(Profile));
                Application.Current.MainPage = new MainPage();

            }
            else
                DependencyService.Get<IToast>().Toasts("createProfile", "failed");
        }
        public void OnSave()
        {
            if (CheckFields())
            {
                var Profile = new Profile()
                {
                    ProfileId = profileId,
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    HouseNumber = houseNumber,
                    Street = street,
                    Barangay = barangay,
                    Town = town,
                    City = city,
                    Birthdate = birthDate,
                    BloodGroup = bloodGroup,
                    OtherInfo = otherInfo
                };
                ProfileDatabase db = new ProfileDatabase(dbPath);
                DependencyService.Get<IToast>().Toasts("updateProfile", db.UpdateProfile(Profile));

            }
            else
                DependencyService.Get<IToast>().Toasts("updateProfile", "failed");
        }
        public bool CheckFields()
        {
            if (firstName != null && lastName != null && houseNumber != null && street != null && barangay != null && town != null && city != null && bloodGroup != null)
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
