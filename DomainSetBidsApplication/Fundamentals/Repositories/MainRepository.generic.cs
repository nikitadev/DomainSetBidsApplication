using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace AviaTicketsWpfApplication.Fundamentals
{
    public sealed class MainRepository<T> : IRepository<T> where T : IDbEntity, new()
	{
		private readonly IDbConnection _dbConnection;

		public SQLiteAsyncConnection Connection
		{  
			get
			{
				return _dbConnection.SQLiteConnection;
            }
		}

		public MainRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
        }

		public async Task InsertAsync(T entity)
		{
			await Connection.InsertAsync(entity);
		}

		public async Task UpdateAsync(T entity)
		{
			await Connection.UpdateAsync(entity);
		}

		public async Task DeleteAsync(T entity)
		{
			await Connection.DeleteAsync(entity);
		}

        public AsyncTableQuery<T> Table
        {
            get
            {
                return Connection.Table<T>();
            }
        }
	}
}
