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
    public class MessageDatabase
    {
        private SQLiteAsyncConnection conn;
        //CREATE  
        public MessageDatabase(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Message>().Wait();
        }

        //READ  
        public Task<Message> GetMessageAsync(string emergency)
        {
            return conn.Table<Message>().Where(i => i.EmergencyName == emergency).FirstOrDefaultAsync();
        }
    
        //ADD
        public string AddMessage(Message message)
        {
            conn.InsertAsync(message);
            return "success";
        }

        //EDIT
        public string UpdateMessage(Message message)
        {
            conn.UpdateAsync(message);
            return "success";
        }

    }
}
