using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
using Rescue.Database;
using System.IO;
namespace Rescue.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfileView : ContentPage
	{
        string dbPath = Path.Combine(
                 Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                 "Rescue.db3");
        public ProfileView ()
		{
            var ProfileViewModel = new ProfileViewModel();
            this.BindingContext = ProfileViewModel;

            InitializeComponent();
            SetData();
            
		}
        public async void SetData()
        {
            ProfileDatabase db = new ProfileDatabase(dbPath);
            var _db = await db.GetProfileAsync();
            if (_db != null)
            {
                string _name = "";
                string _address = "";
                int _age;
                _name += _db.FirstName + " " + ((_db.MiddleName != null) ? _db.MiddleName.ElementAt(0).ToString() : "") + ". " + _db.LastName;
                name.Text = _name;
                _address += _db.HouseNumber + " " + _db.Street + " St. Brgy. " + _db.Barangay + " " + _db.Town + ", " + _db.City;
                address.Text = _address;
                _age = ((DateTime.Now.DayOfYear < _db.Birthdate.DayOfYear) ? DateTime.Now.Year - _db.Birthdate.Year - 1 : DateTime.Now.Year - _db.Birthdate.Year);
                age.Text = _age.ToString();
                blood.Text = _db.BloodGroup;
                other.Text = _db.OtherInfo;
            }
            else
            {
                string _data = "No Data";
                name.Text = _data;
                address.Text = _data;
                age.Text = _data;
                blood.Text = _data;
                other.Text = _data;
            }

        }
	}
}