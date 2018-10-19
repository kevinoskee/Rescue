using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rescue.ViewModel;
namespace Rescue.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditProfileView : ContentPage
	{
		public EditProfileView (string function)
		{
            var editProfileViewModel = new EditProfileViewModel(function);
            this.BindingContext = editProfileViewModel;
            InitializeComponent();
            ChangeButton(function);
		}
        public void ChangeButton(string function)
        {
            switch (function)
            {
                case "create":
                    saveBtn.IsVisible = false;
                    discardBtn.IsVisible = false;
                    Title = "Create Profile";
                    NavigationPage.SetHasBackButton(this, false);
                    break;
                case "edit":
                    Title = "Edit Profile";
                    createBtn.IsVisible = false;
                    break;
            }
        }
 
	}
}