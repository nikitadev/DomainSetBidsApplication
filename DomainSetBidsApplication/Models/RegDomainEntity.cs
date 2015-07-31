using System;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using SQLite;

namespace DomainSetBidsApplication.Models
{
    [Table("RegDomain")]
    public sealed class RegDomainEntity : IDbEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Register { get; set; }

        public decimal Rate { get; set; }

        public int Frequency { get; set; }

        public DateTime Date { get; set; }
    }
}
