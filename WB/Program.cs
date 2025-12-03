using Microsoft.EntityFrameworkCore;
using WB.Data;
using Serilog;
using Serilog.Events;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using WB.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

var builder = WebApplication.CreateBuilder(args);


var _logger = new LoggerConfiguration()
        .WriteTo.File(
            path: "D:\\Poka_Trace_Logs\\log-WB.txt",
            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
            rollingInterval: RollingInterval.Day,
            restrictedToMinimumLevel: LogEventLevel.Error
        ).CreateLogger();

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(_logger));

builder.Services.AddDefaultIdentity<WBUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext1>();



builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddCors();

builder.Logging.AddSerilog(_logger);

try
{
    _logger.Information("Application Is Starting");

}
catch (Exception ex)
{
    _logger.Fatal(ex, "Application Failed to start");
}
finally
{
    Log.CloseAndFlush();
}

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
    {
        // Enable retry policy with Polly
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,         // Maximum number of retry attempts
            maxRetryDelay: TimeSpan.FromMinutes(10), // Delay between retries
            errorNumbersToAdd: null    // SQL Server error numbers to retry on (or null for all)
        );
    });

    // Use the service provider to resolve any dependencies during DbContext creation
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    options.UseLoggerFactory(loggerFactory);
});

// Configure second ApplicationDbContext with Polly retry policy
builder.Services.AddDbContext<ApplicationDbContext1>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptions =>
    {
        // Enable retry policy with Polly
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,         // Maximum number of retry attempts
            maxRetryDelay: TimeSpan.FromMinutes(10), // Delay between retries
            errorNumbersToAdd: null    // SQL Server error numbers to retry on (or null for all)
        );
    });

    // Use the service provider to resolve any dependencies during DbContext creation
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    options.UseLoggerFactory(loggerFactory);
});

builder.Services.AddMemoryCache();
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.SchemaName = "dbo";
    options.TableName = "SessionData";
    options.DefaultSlidingExpiration = TimeSpan.FromDays(30);
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout= TimeSpan.FromHours(5);
    options.Cookie.Name = "MySession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.Cookie.Expiration = TimeSpan.FromDays(365);

});
var app = builder.Build();

//try
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//        dbContext.Database.Migrate(); // Apply pending migrations
//    }

//    app.Run();
//}
//catch (Exception ex)
//{
//    _logger.Fatal(ex, "Database connection or migration error occurred");

//    // Redirect to the DbError page when database connection or migration fails
//    app.Use(async (context, next) =>
//    {
//        context.Response.Redirect("/Category/DbError");
//        await next();
//    });
//}
//finally
//{
//    Log.CloseAndFlush();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Shared/CustomError");
    //app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors( options=>
{
    options.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
});
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseSession();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Manager", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WBUser>>();
    string email = "admin2@bosch.com";
    string email2 = "member@bosch.com";
    string password = "Pokayoke2022";
    string password2 = "Poka-Yoke2022";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new WBUser();
        user.FirstName = email;
        user.LastName = "00112345";
        user.UserName = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }

}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WBUser>>();
    string email2 = "member@bosch.com";
    string password2 = "Poka-Yoke2022";

    if (await userManager.FindByEmailAsync(email2) == null)
    {
        var user2 = new WBUser();
        user2.FirstName = email2;
        user2.LastName = "00112345";
        user2.UserName = email2;

        await userManager.CreateAsync(user2, password2);

        await userManager.AddToRoleAsync(user2, "Member");
    }

}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=Index}/{id?}");

app.Run();
