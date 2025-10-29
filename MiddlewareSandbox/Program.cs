var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

int requestCount = 0;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Use(async (context, next) =>
{
    requestCount++;
    await next();
    await context.Response.WriteAsync($"\nThe amount of processed requests is {requestCount}.");
});

app.Use(async (context, next) =>
{
    if (context.Request.Query.ContainsKey("custom"))
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        await context.Response.WriteAsync("You’ve hit a custom middleware!");
    }
    else
    {
        await next();
    }
});

app.Use(async (context, next) =>
{
    Console.WriteLine($"[{DateTime.Now}] {context.Request.Method} {context.Request.Path}");
    await next();
});

string validApiKey = "12345";

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var key) || key != validApiKey)
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Forbidden: Invalid or missing API key.");
        return;
    }

    await next();
});

app.MapGet("/", () => "Main root");

app.MapGet("/second", () => "Second path Get method");

app.MapPost("/data", async context =>
{
    context.Response.ContentType = "text/plain; charset=utf-8";
    await context.Response.WriteAsync("Post method on the path /data");
});

app.Run();
