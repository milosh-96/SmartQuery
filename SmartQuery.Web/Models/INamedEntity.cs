namespace SmartQuery.Web.Models
{
    public interface INamedEntity
    {
        string Name { get; }
        string Slug { get; }
    }
}
