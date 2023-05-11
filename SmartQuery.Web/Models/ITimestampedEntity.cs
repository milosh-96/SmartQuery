namespace SmartQuery.Web.Models
{
    public interface ITimestampedEntity
    {
        DateTimeOffset CreatedAt { get; set; }
    }
}
