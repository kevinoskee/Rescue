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
	public partial class EntryMessageView
	{
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Rescue.db3");

        public EntryMessageView(string emergency)
		{
            var EntryMessageViewModel = new EntryMessageViewModel(emergency);
            this.BindingContext = EntryMessageViewModel;
            InitializeComponent ();
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