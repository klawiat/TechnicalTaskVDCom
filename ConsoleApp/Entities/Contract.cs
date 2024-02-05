using ConsoleApp.Entities.Enums;

namespace ConsoleApp.Entities
{
    internal class Contract
    {
        public LegalEntity LegalEntity { get; set; }
        public Individual Commissioner { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public DateOnly? DateSigned { get; set; }
    }
}
