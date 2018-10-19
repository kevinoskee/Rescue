using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
using Rg.Plugins.Popup.Services;
using Rescue.Model;
using Rescue.Database;
using System.IO;
using Rescue.View;
namespace Rescue.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntryProfileView
	{
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");

        public EntryProfileView(string function)
        {
            var EntryProfileViewModel = new EntryProfileViewModel(function);
            this.BindingContext = EntryProfileViewModel;
            InitializeComponent();
            ChangeButton(function);
            CloseWhenBackgroundIsClicked = false;

        }
        public void ChangeButton(string function)
        {
            switch (function)
            {
                case "add":
                    addBtn.IsVisible = true;
                    break;
                case "update":
                    updateBtn.IsVisible = true;
                    cancelBtn.IsVisible = true;
                    break;
            }
        }
        public void OnCancel(object s, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        protected override bool OnBackButtonPressed()
        {
            return true; // Disable back button
        }
    
    }
}