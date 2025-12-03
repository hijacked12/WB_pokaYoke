using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WB.Areas.Identity.Data;

namespace WB.Areas.Identity.Data;

public class ApplicationDbContext1 : IdentityDbContext<WBUser>
{
    public ApplicationDbContext1(DbContextOptions<ApplicationDbContext1> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
