using CapyAuth.Api;

public class Program
{
    private static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateWebHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}
