using SQLite;
using System.Threading.Tasks;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
	public interface IRepository<T> where T : IDbEntity, new()
	{
		SQLiteAsyncConnection Connection { get; }

		Task InsertAsync(T obj);

		Task UpdateAsync(T obj);

		Task DeleteAsync(T obj);

        AsyncTableQuery<T> Table { get; }
	}
}
