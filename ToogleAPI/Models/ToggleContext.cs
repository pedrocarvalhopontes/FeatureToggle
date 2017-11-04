using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToggleAPI.Models
{
    public class ToggleContext : IdentityDbContext
    {
        public DbSet<Toggle> ToggleItems { get; set; }

        public ToggleContext(DbContextOptions<ToggleContext> options)
            : base(options)
        {
        }      
    }
}
