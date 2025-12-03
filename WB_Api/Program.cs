using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using WB_Api.Data;
using WB_Api.Configurations;
using Microsoft.Extensions.DependencyInjection;
using WB_Api.IRepository;
using WB_Api.Repository;
using Microsoft.AspNetCore.Identity;
using WB_Api;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoMapper;
using WB_Api.Services;

internal class Program
{
    private static IConfiguration Configuration{ get; }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        // Add services to the container.
        var _logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: "C:\\Users\\OLJ1SGH\\Desktop\\New folder\\WB\\logs\\log-API.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                ).CreateLogger();

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
        builder.Services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureVersioning();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(o =>
        {
            o.AddPolicy("AllowAll", build =>
                build.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });
        builder.Services.AddAutoMapper(typeof(MapperInitializer));
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IAuthManager, AuthManager>();
        builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ));


        builder.Services.AddAuthentication();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJWT(Configuration);

        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseSerilog();
        app.ConfigureExceptionHandler();
        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}