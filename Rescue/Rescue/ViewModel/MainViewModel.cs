using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Rescue.Model;
using Plugin.Geolocator;
using System.Threading.Tasks;
namespace Rescue.ViewModel
{
    public class MainViewModel
    {
        //public LocationModel locationModel = new LocationModel { };
        //public PersonModel personModel = new PersonModel { };
        //public MessageModel messageModel = new MessageModel { };
        //public ICommand SendSMS { protected set; get; }

        //public MainViewModel()
        //{
        //    SendSMS = new Command(OnSendSMS);
        //}

        //public void OnSendSMS()
        //{
        //   //await DependencyService.Get<IGetLocation>().Location();
        //    GetNumber();
        //    GetDefaultMessage();
        //  // DependencyService.Get<ISendSMS>().Message(personModel.Number,messageModel.Message);
        //   DependencyService.Get<ISendSMS>().Send(personModel.Number, messageModel.Message);
        //}
        //public void GetDefaultMessage()
        //{
        //    messageModel.Message = personModel.Name + "\nHelp me!\nI am at : ";
        //}
        //public void GetNumber() 
        //{
        //    personModel.Number = "09478227779";
        //    personModel.Name = "Rescue Team";

        //}
    }
}
