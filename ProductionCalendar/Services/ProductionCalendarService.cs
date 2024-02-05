using Microsoft.EntityFrameworkCore;
using ProductionCalendar.Data;
using ProductionCalendar.Interfaces;
using ProductionCalendar.Models;

namespace ProductionCalendar.Services
{
    public class ProductionCalendarService : IProductionCalendarService
    {
        private readonly ProductionCalendarDbContext context;
        public ProductionCalendarService(ProductionCalendarDbContext context)
        {
            this.context = context;
        }

        public async Task<string?> GetInformationAboutTheDay(DateOnly date)
        {
            Calendar? desiredDay = await context.days.FirstOrDefaultAsync(day => day.Date == date);
            return desiredDay is null ? null : desiredDay.TypeOfDay;
        }

        public async Task UpdateDates(int year)
        {
            string link = @$"https://isdayoff.ru/api/getdata?year={year}&pre=1";
            string result;
            DateOnly firstDate = DateOnly.Parse($"01.01.{year}");
            using (HttpClient client = new HttpClient())
            {
                result = await client.GetStringAsync(link);
            }
            for (int i = 1; i <= result.Length; i++)
            {
                Calendar calendar = new Calendar();
                switch (result[i - 1])
                {
                    case '0':
                        calendar.TypeOfDay = "Рабочий день";
                        break;
                    case '1':
                        calendar.TypeOfDay = "Нерабочий день";
                        break;
                    case '2':
                        calendar.TypeOfDay = "Сокращённый рабочий день";
                        break;
                    case '4':
                        calendar.TypeOfDay = "Рабочий день";
                        break;
                    default:
                        calendar.TypeOfDay = "Неизвестно";
                        break;
                }
                calendar.DayOfTheWeek = ((int)firstDate.AddDays(i - 1).DayOfWeek);
                calendar.Date = firstDate.AddDays(i - 1);
                context.days.Add(calendar);
            }
            await context.SaveChangesAsync();
        }
    }
}
