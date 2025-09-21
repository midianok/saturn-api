using Saturn.Api.Extensions;
using Saturn.Application;
using Saturn.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterServices()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>())
    .AddAutoMapper(x => x.AddMaps(typeof(ApplicationAssemblyMarker).Assembly))
    .AddSaturnContext(builder.Configuration.GetConnectionString("Saturn"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.ApplyMigrations();
app.Run();