using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiStudyBuddy.Data;
using ApiStudyBuddy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApiStudyBuddyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApiStudyBuddyContext") ?? throw new InvalidOperationException("Connection string 'ApiStudyBuddyContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//};

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapUserEndpoints();

app.MapDeckEndpoints();

app.MapDeckFlashCardEndpoints();

app.MapDeckGroupEndpoints();

app.MapDeckGroupDeckEndpoints();

app.MapFlashCardEndpoints();

app.MapStudySessionEndpoints();

app.MapStudySessionFlashCardEndpoints();

app.MapUserDeckEndpoints();

app.MapUserDeckGroupEndpoints();

app.Run();
