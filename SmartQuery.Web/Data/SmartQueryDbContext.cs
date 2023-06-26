
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Models;
using System.Reflection.Emit;
using System;

namespace SmartQuery.Web.Data
{
    public class SmartQueryDbContext : IdentityDbContext<WebUser, WebRole, int>
    {
        public SmartQueryDbContext(DbContextOptions<SmartQueryDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Entry>()
                .HasMany(e => e.Adjectives).WithMany(e => e.Entries)
                .UsingEntity<AdjectiveEntry>();

        
            

            builder.Entity<AdjectiveEntry>().HasIndex(x => new { x.AdjectiveId, x.EntryId }).IsUnique();
            
            
            builder.Entity<EntryEntry>().HasIndex(x => new { x.EntryId, x.RelatedEntryId }).IsUnique();
           
            
                




        }
        public DbSet<Entry> Entries {get;set;} 
      
        public DbSet<Adjective> Adjectives {get;set;} 
    }
}