using System;
using System.Collections.Generic;
using DomainSetBidsApplication.Fundamentals.Abstracts;
using SQLite;

namespace DomainSetBidsApplication.Models
{
    [Table("Log")]
    public sealed class LogEntity : BaseRegDomainEntity
    {
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public LogType Type { get; set; }

        public override string ToString()
        {
            var elements = new List<string>();
            elements.Add(Date.ToShortDateString());
            elements.Add(Name);
            elements.Add(Register);
            elements.Add(Rate.ToString());
            elements.Add(Enum.GetName(typeof(LogType), Type));
            elements.Add(Description);

            return String.Join("\t", elements.ToArray());
        }
    }
}
