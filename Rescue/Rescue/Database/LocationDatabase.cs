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
    public class LocationDatabase
    {
        private SQLiteAsyncConnection conn;
        //CREATE  
        public LocationDatabase(string dbPath)
        {
            conn = new SQLiteAsyncConnection(dbPath);
            conn.CreateTableAsync<Location>().Wait();
        }

        //READ  
        public Task<Location> GetPrevLocationAsync()
        {
            return conn.Table<Location>().FirstOrDefaultAsync();
        }

        //INSERT  
        public void AddLocation(Location location)
        {
            conn.InsertAsync(location);
        }


        //CLEAR
        public void ClearLocation()
        {
            conn.DeleteAllAsync<Location>();
        }
    }
}
