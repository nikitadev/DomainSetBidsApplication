using System;

namespace AviaTicketsWpfApplication.Models
{
    public sealed class PageMessage
    {
        public Type TypeViewModel { get; private set; }

        public string Parametrs { get; private set; }

        public PageMessage(Type type)
        {
            TypeViewModel = type;
        }

        public PageMessage(Type type, string parametrs)
        {
            TypeViewModel = type;

            Parametrs = parametrs;
        }
    }
}
