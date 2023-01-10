using WildberriesPriceScaner;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MainService>();
var app = builder.Build();
var t = new MainService();
t.Main();
app.MapGet("/", () => "Hello World!");

app.Run();
