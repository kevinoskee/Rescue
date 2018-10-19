using System;
using System.Collections.Generic;
using System.Text;
using Rescue.ViewModel;
using SQLite;
namespace Rescue.Model
{
    [Table("Location")]
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int LocationId { get; set; }
        public string CurrentLocation { get; set; }
    }
    [Table("Contact")]
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int ContactId { get; set; }
        public string EmergencyName { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
    }
    [Table("Message")]
    public class Message
    {
        [PrimaryKey]
        public string EmergencyName { get; set; }
        public string MessageTemplate { get; set; }
    }
    [Table("Profile")]
    public class Profile
    {
        [PrimaryKey,AutoIncrement]
        public int ProfileId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string BloodGroup { get; set; }
        public string Medications { get; set; }
        public string Allergies { get; set; }
        public string OtherInfo { get; set; }

    }

}
