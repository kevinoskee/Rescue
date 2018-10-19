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
    public class ProfileDatabase
    {
        private SQLiteAsyncConnection conn;
        //CREATE  
        public ProfileDatabase(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Profile>().Wait();
        }

        //READ  
        public Task<Profile> GetProfileAsync()
        {
            return conn.Table<Profile>().FirstOrDefaultAsync();
        }

        //INSERT  
        public string AddProfile(Profile profile)
        {
            conn.InsertAsync(profile);
            return "success";
        }
        //DELETE  
        public string DeleteProfile(int id)
        {
            conn.DeleteAsync<Profile>(id);
            return "success";
        }
        //EDIT
        public string UpdateProfile(Profile profile)
        {
            conn.UpdateAsync(profile);
            return "success";
        }

        //CHECK
        public string CheckProfileAsync()
        {
            var entryCount = conn.Table<Profile>().CountAsync();
            if (entryCount != null)
                return "Has Profile";
            else
                return "No Profile";
        }

    }
}
