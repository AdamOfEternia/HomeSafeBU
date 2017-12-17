using HomeSafe.Droid.Persistence;
using Xamarin.Forms;
using SQLite;
using HomeSafe.Persistence;

[assembly: Dependency(typeof(SQLiteDb))]
namespace HomeSafe.Droid.Persistence
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = System.IO.Path.Combine(documentsPath, "HSafeSQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}