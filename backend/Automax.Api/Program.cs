using Automax.Api.Configuration;
using Automax.Api.Data;
using Automax.Api.Repositories;
using Automax.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FakeStoreOptions>(
    builder.Configuration.GetSection(FakeStoreOptions.SectionName));

builder.Services.AddDbContext<AutomaxDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddHttpClient<ICartSyncService, CartSyncService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AutomaxDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Frontend");
app.MapControllers();

app.Run();

public partial class Program
{
}
