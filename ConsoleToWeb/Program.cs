internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Це вебзастосунок, що створений з консольного.");

        app.Run();
    }
}