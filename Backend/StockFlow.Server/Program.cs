using StockFlow.Server.Configurations;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddCustomDatabaseConfiguration(configuration);

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
