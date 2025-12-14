using Saturn.Api.Extensions;
using Saturn.Application;
using Saturn.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterServices()
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblyContaining<ApplicationAssemblyMarker>();
        cfg.LicenseKey = builder.Configuration.GetValue<string>("LicenseKey");
    })
    .AddAutoMapper(cfg =>
    {
        cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
        cfg.LicenseKey = builder.Configuration.GetValue<string>("LicenseKey");
    })
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