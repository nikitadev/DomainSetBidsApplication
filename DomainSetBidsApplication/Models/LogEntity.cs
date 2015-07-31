using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace DomainSetBidsApplication.Models
{
    [Table("Log")]
    public sealed class LogEntity : IDbEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Name { get; set; }

        public string Description { get; set; }

        public LogType Type { get; set; }
    }
}
