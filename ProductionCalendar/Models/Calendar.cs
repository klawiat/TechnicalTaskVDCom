namespace ProductionCalendar.Models
{
    public class Calendar
    {
        public DateOnly Date { get; set; }
        public int DayOfTheWeek { get; set; }
        public string TypeOfDay { get; set; }
    }
}
