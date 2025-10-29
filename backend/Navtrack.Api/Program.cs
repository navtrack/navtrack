using Microsoft.AspNetCore.Builder;
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
                DbContextUtils.AddDbContext<NavtrackDbContext>(webApplicationBuilder.Services,
                    webApplicationBuilder.Configuration);
            },
            MigrateDatabase = true
        });
    }
}