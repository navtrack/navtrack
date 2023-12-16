using Navtrack.Api.Shared;

namespace Navtrack.Api;

public class Program
{
    public static void Main(string[] args)
    {
        BaseApiProgram<Program>.Main(args, typeof(Program).Assembly);
    }
}