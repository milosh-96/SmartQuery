using System.ComponentModel.DataAnnotations;

namespace SmartQuery.Web.Models
{
    public class Adjective : IEntity, INamedEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }

        public List<Entry> Entries { get; set; } = new List<Entry>();
    }
}
