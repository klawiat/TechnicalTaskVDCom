using ConsoleApp.Entities.Abstract;

namespace ConsoleApp.Entities
{
    internal class LegalEntity : EntityBase
    {
        public string Inn { get; set; }
        public string Ogrn { get; set; }
    }
}
