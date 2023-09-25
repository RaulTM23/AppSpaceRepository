using AppSpace;
using AppSpace.Business.Interfaces;
using AppSpace.Business.Services;
using AppSpace.Domain.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

//DependencyInjectionSetup class functions
builder.Services.CreateDbContext(configuration);
builder.Services.CreateMovieApiHttpClient(configuration);
builder.Services.RegisterServices();

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
