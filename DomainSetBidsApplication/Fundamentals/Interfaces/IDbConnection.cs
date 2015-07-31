using SQLite;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
	public interface IDbConnection
	{
		SQLiteAsyncConnection SQLiteConnection { get; }
	}
}
