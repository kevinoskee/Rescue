using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
using Rescue.Model;
using Rescue.Database;
using System.IO;

namespace Rescue.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpView : ContentPage
    {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");
        public bool hasEntry = false;

        public string emergencyName;


        public SetUpView(string emergency)
        {
            InitializeComponent();
            CountContact("Init",emergency);
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnContactAdded", (sender) =>
            {
                NewContact(emergency);
            });
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnContactUpdated", (sender) =>
            {
                ShowContact(emergency);
            });
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnContactDeleted", (sender) =>
            {
                CountContact("Delete",emergency);
            });
            ToolbarItems.Add(new ToolbarItem("Message", "message.png", async () => {
                await PopupNavigation.Instance.PushAsync(new EntryMessageView(emergency));}));
        }
     
        public async void ShowContact(string emergency)
        {
            contactList.Children.Clear();
            noContact.IsVisible = false;
            contacts.IsVisible = true;
            ContactDatabase db = new ContactDatabase(dbPath);
            var list = await db.GetContactsAsync(emergency);
            foreach (Contact contact in list)
            {
                NewContact newContact = new NewContact(emergency);
                newContact.contactId.Text = contact.ContactId.ToString();
                newContact.contactName.Text = contact.ContactName;
                newContact.contactNumber.Text = contact.ContactNumber;
                contactList.Children.Add(newContact);
            }           
        }

        public async void NewContact(string emergency)
        {
            noContact.IsVisible = false;
            contacts.IsVisible = true;
            ContactDatabase db = new ContactDatabase(dbPath);
            var contact = await db.GetNewContactAsync(emergency);
          
                NewContact newContact = new NewContact(emergency);
                newContact.contactId.Text = contact.ContactId.ToString();
                newContact.contactName.Text = contact.ContactName;
                newContact.contactNumber.Text = contact.ContactNumber;
                contactList.Children.Add(newContact);
          
        }





        public async void CountContact(string function, string emergency)
        {
            int contactCount = 0;
            ContactDatabase db = new ContactDatabase(dbPath);
            contactCount = await db.CountContact(emergency);

            if (contactCount > 0)
            {
                if(function == "Init")
                    ShowContact(emergency);
            }

            else
            {
                noContact.IsVisible = true;
                contacts.IsVisible = false;
            }
            //   
        }

         public void OnNew(object s, EventArgs e)
        {
           PopupNavigation.Instance.PushAsync(new EntryContactView("add", emergencyName));
 
        }
        public void OnRefresh(object s, EventArgs e)
        {
            ShowContact(emergencyName);
        }
        

    }
}
