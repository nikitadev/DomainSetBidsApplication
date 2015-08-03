using System;
using DomainSetBidsApplication.Fundamentals.Abstracts;
using SQLite;

namespace DomainSetBidsApplication.Models
{
    [Table("RegDomain")]
    public sealed class RegDomainEntity : BaseRegDomainEntity
    {
        public int Frequency { get; set; }

        public DateTime? Date { get; set; }

        public int? Hour { get; set; }

        public int? Minute { get; set; }

        public int? Second { get; set; }
    }
}
