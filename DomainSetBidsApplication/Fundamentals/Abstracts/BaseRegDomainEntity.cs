using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace DomainSetBidsApplication.Fundamentals.Abstracts
{
    public abstract class BaseRegDomainEntity : IDbEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Name { get; set; }

        public string Register { get; set; }

        public int Rate { get; set; }
    }
}
