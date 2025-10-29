var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// main
app.MapGet("/", () =>
Results.Content(
    """
    <meta charset="utf-8"/>
    <h1>Головна сторінка</h1>
    <a href='/who'>who</a><br/>
    <a href='/time'>time</a>
    """,
        "text/html")
);

// GET /who
app.MapGet("/who", () => new { Name = "Ірина", Surname = "Клосович" });

// GET /time
app.MapGet("/time", () => new { ServerTime = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") });

app.Run();


