using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionCalendar.Data;
using ProductionCalendar.Interfaces;
using ProductionCalendar.Services;
using System.Net;
namespace ProductionCalendar
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProductionCalendarDbContext>(
                opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Transient);
            builder.Services.AddScoped<IProductionCalendarService, ProductionCalendarService>();
            builder.Services.AddTransient<ExceptionHandler>();

            WebApplication app = builder.Build();

            app.UseMiddleware<ExceptionHandler>();

            app.MapGet("/", () => Results.Redirect("/31/01/2024"));

            app.MapGet("/{day}/{month}/{year}",
                async (int day, int month, int year, HttpContext context, [FromServices] IProductionCalendarService calendar) =>
                {
                    DateOnly requestedDate;
                    if (!DateOnly.TryParseExact($"{day.ToString("00")}.{month.ToString("00")}.{year.ToString("0000")}", "dd.MM.yyyy", out requestedDate))
                        return Results.BadRequest("Неправильный формат даты");
                    //Console.WriteLine(requestedDate.Day.ToString() + "\n\n");
                    string? typeDay;
                    if ((typeDay = await calendar.GetInformationAboutTheDay(requestedDate)) is null)
                        await calendar.UpdateDates(requestedDate.Year);
                    return Results.Ok(typeDay ?? await calendar.GetInformationAboutTheDay(requestedDate));
                });
            app.MapGet("/{day}",
                async (int day) =>
                {
                    return Results.Ok($"Сегодня {day} день");
                });
            app.Run();
        }
    }
    public class ExceptionHandler:IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                next?.Invoke(context);
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Response.ContentType = "application/text;charset=utf-8";
                await context.Response.WriteAsync("Произошла непредвиденная ошибка");
            }
        }
    }
}
