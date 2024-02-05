namespace ConsoleApp.Entities.Abstract
{
    internal abstract class EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Сountry { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
