﻿using System.ComponentModel.DataAnnotations;

namespace SmartQuery.Web.Models
{
    public class Entry : INamedEntity,IEntity,ITimestampedEntity
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }

        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public List<Adjective> Adjectives { get; set; } = new List<Adjective>();


        public List<Entry> Entries { get; set; } = new List<Entry>();
        public List<Entry> RelatedTo { get; set; } = new List<Entry>();
    }
}
