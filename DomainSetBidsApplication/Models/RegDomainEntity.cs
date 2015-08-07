using System;
using System.Collections.Generic;
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

        public RegDomainState State { get; set; }

        public override string ToString()
        {
            var elements = new List<string>();
            elements.Add(Name);
            elements.Add(Register);
            elements.Add(Rate.ToString());

            if (Date.HasValue)
            {
                elements.Add(Date.Value.ToShortDateString());
            }

            if (Hour.HasValue && Minute.HasValue && Second.HasValue)
            {
                var timeSpan = new TimeSpan(Hour.Value, Minute.Value, Second.Value);
                elements.Add(timeSpan.ToString());
            }

            elements.Add(State.ToLocalString());

            return String.Join("\t", elements.ToArray());
        }
    }
}
