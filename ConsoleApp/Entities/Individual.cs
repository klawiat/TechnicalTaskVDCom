using ConsoleApp.Entities.Abstract;
using ConsoleApp.Entities.Enums;

namespace ConsoleApp.Entities
{
    internal class Individual : EntityBase
    {
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public Sex Sex { get; set; }
        public int Age { get; set; }
        public string Job { get; set; }
        public DateOnly Birthday { get; set; }
    }
}
