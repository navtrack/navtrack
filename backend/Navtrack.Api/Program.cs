using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Navtrack.Api.Shared;
using Navtrack.Database.Model;

namespace Navtrack.Api;

public class Program
{
    public static void Main(string[] args)
    {
        BaseApiProgram<Program>.Main(args, typeof(Program).Assembly, new BaseProgramOptions
        {
            ConfigureServices = delegate(WebApplicationBuilder webApplicationBuilder)
            {
                webApplicationBuilder.Services.AddDbContext<DbContext, NavtrackDbContext>(opt =>
                    opt.UseNpgsql(
                        webApplicationBuilder.Configuration.GetConnectionString("Postgres")));
            },
            MigrateDatabase = true
        });
    }
}