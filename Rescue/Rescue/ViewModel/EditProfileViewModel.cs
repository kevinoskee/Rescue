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
        int profileId;
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
            }
        }
        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                middleName = value;
            }
        }
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
               lastName = value;
            }
        }
        private string houseNumber;
        public string HouseNumber
        {
            get { return houseNumber; }
            set
            {
                houseNumber = value;
            }
        }
        private string street;
        public string Street
        {
            get { return street; }
            set
            {
                street = value;
            }
        }
        private string barangay;
        public string Barangay
        {
            get { return barangay; }
            set
            {
                barangay = value;
            }
        }
        private string town;
        public string Town
        {
            get { return town; }
            set
            {
               town = value;
            }
        }
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                city = value;
            }
        }
        private DateTime birthDate = DateTime.Parse((Convert.ToInt32(DateTime.Now.Year) - 1).ToString() + '/' + DateTime.Now.Month + '/' + DateTime.Now.Day);
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
            }
        }
        private string bloodGroup;
        public string BloodGroup
        {
            get { return bloodGroup; }
            set
            {
                bloodGroup = value;
            }
        }
        private string otherInfo;
        public string OtherInfo
        {
            get { return otherInfo; }
            set
            {
                otherInfo = value;
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
            var _db = await db.GetProfileAsync();
            profileId = _db.ProfileId;
            firstName = _db.FirstName;
            middleName = _db.MiddleName;
            lastName = _db.LastName;
            houseNumber = _db.HouseNumber;
            street = _db.Street;
            barangay = _db.Barangay;
            town = _db.Town;
            city = _db.City;
            birthDate = _db.Birthdate;
            bloodGroup = _db.BloodGroup;
            otherInfo = _db.OtherInfo;
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
                ShowAlert(db.AddProfile(Profile), "create");
               
            }
            else
                ShowAlert("failed","create");
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
                ShowAlert(db.UpdateProfile(Profile), "edit");

            }
            else
                ShowAlert("failed","edit");
        }
        public bool CheckFields()
        {
            if (firstName != null && lastName != null && houseNumber != null && street != null && barangay != null && town != null && city != null && bloodGroup != null)
                return true;
            else
                return false;
        }

    }
}
