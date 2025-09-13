using Saturn.Api.Extensions;
using Saturn.Application;
using Saturn.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>());
builder.Services.AddAutoMapper(x => x.AddMaps(typeof(ApplicationAssemblyMarker).Assembly));
builder.Services.AddSaturnContext(builder.Configuration.GetConnectionString("Saturn"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.ApplyMigrations();
app.Run();