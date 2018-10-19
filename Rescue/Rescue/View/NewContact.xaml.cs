using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rescue;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
using Rescue.Model;
using System.ComponentModel;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Rescue.View;
using Rescue.Database;
using SQLite;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Rescue.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewContact : ContentView
	{
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");
        string emergencyName;
        public NewContact (string emergency)
		{
            InitializeComponent();
            emergencyName = emergency;
		}
        public void OnSwiped(object s, EventArgs e)
        {
                this.IsVisible = false;
                ContactDatabase db = new ContactDatabase(dbPath);
           
            DependencyService.Get<IToast>().Toasts("deleteContact", db.DeleteContact(Convert.ToInt32(this.contactId.Text)));
            MessagingCenter.Send<App>((App)Application.Current, "OnContactDeleted");

        }
        public void OnTapped(object s, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new EntryContactView("update",emergencyName,Convert.ToInt32(contactId.Text)));
      
        }
     

    }
}