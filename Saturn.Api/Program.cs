using Mapster;
using Saturn.Api.Extensions;
using Saturn.Application;
using Saturn.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMapster();

builder.Services
    .RegisterServices()
    .AddSaturnContext(builder.Configuration.GetConnectionString("Saturn"));

TypeAdapterConfig.GlobalSettings.Scan(typeof(ApplicationMarker).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.ApplyMigrations();
app.Run();