using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using SQLite;
using Rescue.Model;
using System.Linq;
using Xamarin.Forms;
using System.Collections;
namespace Rescue.Database
{
    public class ContactDatabase
    {
        private SQLiteAsyncConnection conn;
        //CREATE  
        public ContactDatabase(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Contact>().Wait();
        }

        //READ  
        public Task<Contact> GetContactAsync(string emergency,int id = 0)
        {
            return conn.Table<Contact>().Where(i => i.EmergencyName == emergency && i.ContactId == id).FirstOrDefaultAsync();
        }
        public Task<Contact> GetNewContactAsync(string emergency)
        {
            return conn.Table<Contact>().Where(i => i.EmergencyName == emergency).OrderByDescending(t => t.ContactId).FirstOrDefaultAsync();
        }

        public Task<List<Contact>> GetContactsAsync(string emergency)
        {
            return conn.Table<Contact>().Where(i => i.EmergencyName == emergency).ToListAsync();
        }
        //READ  
        public Task<int> CountContact(string emergency)
        {
            return conn.Table<Contact>().Where(i => i.EmergencyName == emergency).CountAsync();
        }
       
        //INSERT  
        public string AddContact(Contact contact)
        {
            conn.InsertAsync(contact);
            return "success";
        }
        //DELETE  
        public string DeleteContact(int id)
        {
            conn.DeleteAsync<Contact>(id);
            return "success";
        }

        //EDIT
        public string UpdateContact(Contact contact)
        {
            conn.UpdateAsync(contact);
            return "success";
        }

        

    }
}
