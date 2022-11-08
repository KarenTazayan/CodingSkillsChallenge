using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using TestingControllersSample.Core.Interfaces;
using TestingControllersSample.Core.Model;
using TestingControllersSample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    optionsBuilder => optionsBuilder.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBrainstormSessionRepository>(serviceProvider =>
{
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
    var repository = new BrainstormSessionRepository(dbContext);
    InitializeDatabaseAsync(repository);
    return repository;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();


static void InitializeDatabaseAsync(IBrainstormSessionRepository repo)
{
    var sessionList = repo.ListAsync().Result;
    if (!sessionList.Any())
    {
        repo.AddAsync(GetTestSession()).Wait();
    }

    static BrainstormSession GetTestSession()
    {
        var session = new BrainstormSession
        {
            Name = "Sample Session 1",
            DateCreated = new DateTime(2016, 8, 1)
        };
        var idea = new Idea
        {
            DateCreated = new DateTime(2016, 8, 1),
            Description = "Totally awesome idea",
            Name = "Awesome idea"
        };
        session.AddIdea(idea);
        return session;
    }
}