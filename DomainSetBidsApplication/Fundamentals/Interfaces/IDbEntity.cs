namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IDbEntity
    {
        int Id { get; set; }

        string Name { get; set; }
    }
}
