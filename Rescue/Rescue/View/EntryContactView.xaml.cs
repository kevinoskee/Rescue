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
	public partial class EntryContactView
	{
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");

        public EntryContactView(string function, string emergency, int id = 0)
		{
            var EntryContactViewModel = new EntryContactViewModel(function,emergency,id);
            this.BindingContext = EntryContactViewModel;
            InitializeComponent ();
            ChangeButton(function);

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
                    break;
            }
        }
        public void OnCancel(object s, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
        protected override bool OnBackButtonPressed()
        {
            PopupNavigation.Instance.PopAsync(true);
            return true;
        }
      


    }
}