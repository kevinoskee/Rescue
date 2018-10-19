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
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Barangay { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public DateTime Birthdate { get; set; }
        public string BloodGroup { get; set; }
        //public string Medications { get; set; }
        //public string Allergies { get; set; }
        public string OtherInfo { get; set; }

    }

}
