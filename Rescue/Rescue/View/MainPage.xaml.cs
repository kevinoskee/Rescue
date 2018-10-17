using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rescue.ViewModel;
namespace Rescue.View
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
            /* var mainviewModel = new MainViewModel();
             this.BindingContext = mainviewModel;*/
            InitializeComponent();
            var detail = new NavigationPage(new HomeView())
            {
                BarBackgroundColor = Color.FromHex("#34495e")
            };
            var master = new NavigationPage(new ProfileView())
            {
                BarBackgroundColor = Color.FromHex("#2c3e50"),
                Title="Master",
            };
            this.Master = master;
            this.Detail = detail;
            App.MasterDetail = this;



        }
	}
}
