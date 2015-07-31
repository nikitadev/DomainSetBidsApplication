using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace DomainSetBidsApplication.Fundamentals
{
    public class DbConnection : IDbConnection
	{
		public SQLiteAsyncConnection SQLiteConnection { get; private set; }

		public DbConnection(string dbName)
		{
			SQLiteConnection = new SQLiteAsyncConnection(dbName);
        }
	}
}
