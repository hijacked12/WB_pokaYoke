using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WB.Models;
using WB_Api.Models;

namespace WB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ScanLog> ScanLog { get; set; }
        public DbSet<Category> Categories1 { get; set; }
        public DbSet<ScannerDTO> scanner { get; set; }
        public DbSet<state_change> change_state { get; set; }


    }
}
