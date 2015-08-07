using DomainSetBidsApplication.Properties;

namespace DomainSetBidsApplication.Models
{
    public enum RegDomainState { None, Draft, Done, Cancel, Pending, Working }

    public static class RegDomainStateEx
    {
        public static string ToLocalString(this RegDomainState state)
        {
            switch (state)
            {
                case RegDomainState.Draft:
                    return Resources.Draft;
                case RegDomainState.Cancel:
                    return Resources.Cancel;
                case RegDomainState.Pending:
                    return Resources.Pending;
                case RegDomainState.Working:
                    return Resources.Working;
                case RegDomainState.Done:
                    return Resources.Done;                   
            }

            return Resources.None;
        }
    }
}