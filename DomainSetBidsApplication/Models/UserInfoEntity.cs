using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace DomainSetBidsApplication.Models
{
    [Table("UserInfo")]
    public sealed class UserInfoEntity : IDbEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Data { get; set; }
    }
}
