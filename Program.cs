using CryptocurrencyAPI.Data.Contexts;
using CryptocurrencyAPI.Data.Interfaces;
using CryptocurrencyAPI.Data.Repositories;
using CryptocurrencyAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

const string connection = @"Server=db;Database=CryptocurrencyAPI;User=sa;Password=Cryptocurrency123;";

builder.Services.AddDbContext<MainDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddScoped<ICryptocurrencyInformationRepository, CryptocurrencyInformationRepository>();
builder.Services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
builder.Services.AddScoped<IRestApi, CoinApiWorker>();
builder.Services.AddSingleton<IWebSocket, CoinWebSocketWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

InitDb.InitData(app, builder.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(b => { b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });

app.Run();