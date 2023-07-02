using System.ComponentModel.DataAnnotations.Schema;

namespace SmartQuery.Web.Models
{
    public class EntryEntry : IEntity
    {
        public int Id { get; set; }

        public int EntryId { get; set; }
        public Entry Entry { get; set; }

        public int RelatedEntryId { get; set; }
        public Entry RelatedEntry { get; set; }




    }
}
