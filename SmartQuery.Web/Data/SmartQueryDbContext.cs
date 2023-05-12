
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
        }
        public DbSet<Entry> Entries {get;set;} 
        public DbSet<Adjective> Adjectives {get;set;} 
    }
}