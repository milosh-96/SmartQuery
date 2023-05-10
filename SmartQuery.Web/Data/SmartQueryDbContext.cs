
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartQuery.Web.Models;

namespace SmartQuery.Web.Data
{
    public class SmartQueryDbContext : IdentityDbContext<WebUser,WebRole,int>
    {
        public SmartQueryDbContext(DbContextOptions<SmartQueryDbContext> options)
            : base(options)
        {
        }
    }
}