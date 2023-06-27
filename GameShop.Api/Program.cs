using GameShop.Core.Interfaces;
using GameShop.Infrastructure.EntityFramework;
using GameShop.Infrastructure.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<GameShopContext>(options =>
    options.UseSqlite("DataSource=gameshop.db")
        .EnableSensitiveDataLogging()
        .LogTo(message => Debug.Write(message))
);

builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();