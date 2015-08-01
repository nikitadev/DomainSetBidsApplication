using System;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using Newtonsoft.Json;
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

        public int Rate { get; set; }

        public int Frequency { get; set; }

        public DateTime Date { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }
    }
}
