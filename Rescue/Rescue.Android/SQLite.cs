using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Rescue.Interface;
using Rescue.Droid;
using System.IO;
using Xamarin.Forms;
using Android.OS;

[assembly: Dependency(typeof(Droid_SQLite))]

namespace Rescue.Droid
{
    public class Droid_SQLite : ISQLite
    {
        public SQLite.SQLiteAsyncConnection GetConnection()
        {
            var dbName = "Rescue.db3";
            var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(dbPath, dbName);
            var conn = new SQLite.SQLiteAsyncConnection(path);
            return conn;
        }
    }
}