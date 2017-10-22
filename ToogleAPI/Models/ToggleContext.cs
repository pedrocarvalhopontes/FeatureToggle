using Microsoft.EntityFrameworkCore;

namespace ToogleAPI.Models
{
    public class ToggleContext : DbContext
    {
        public DbSet<Toggle> ToggleItems { get; set; }

        public ToggleContext(DbContextOptions<ToggleContext> options)
            : base(options)
        {
        }      
    }
}
