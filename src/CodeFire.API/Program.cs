using CodeFire.API.Extensions;
using CodeFire.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.ConfigureSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

// Create the app
var app = builder.Build();

// Initialize the database
InfrastructureServiceRegistration.InitializeDatabase(app.Services);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
} 