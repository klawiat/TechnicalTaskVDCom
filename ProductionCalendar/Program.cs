using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductionCalendar.Data;
using ProductionCalendar.Interfaces;
using ProductionCalendar.Services;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
namespace ProductionCalendar
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProductionCalendarDbContext>(
                opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);
            builder.Services.AddScoped<IProductionCalendarService, ProductionCalendarService>();

            var app = builder.Build();

            app.MapGet("/",()=>Results.Redirect("/31.01.2024"));

            app.MapGet("/{day}.{month}.{year}", async (int day, int month, int year, [FromServices] ProductionCalendarDbContext dbContext, [FromServices] IProductionCalendarService calendar) =>
            {
                //Console.WriteLine($"{day.ToString("00")}.{month.ToString("00")}.{year.ToString("0000")}");
                DateOnly dateOnly = DateOnly.ParseExact($"{day.ToString("00")}.{month.ToString("00")}.{year.ToString("0000")}", "dd.MM.yyyy");
                Console.WriteLine(dateOnly.Day.ToString()+"\n\n");
                string typeDay;
                if ((typeDay = await calendar.GetInformationAboutTheDay(dateOnly)) is null)
                    await calendar.UpdateDates(dateOnly.Year);
                return await calendar.GetInformationAboutTheDay(dateOnly);
            });
            app.Run();
        }
    }
}
