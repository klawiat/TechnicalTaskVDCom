namespace ProductionCalendar.Interfaces
{
    public interface IProductionCalendarService
    {
        public Task UpdateDates(int year);
        public Task<string?> GetInformationAboutTheDay(DateOnly date);
    }
}
