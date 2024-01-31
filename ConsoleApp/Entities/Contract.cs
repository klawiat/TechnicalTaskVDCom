using ConsoleApp.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Entities
{
    internal class Contract
    {
        public LegalEntity LegalEntity { get; set; }
        public Individual Commissioner {  get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public DateOnly? DateSigned { get; set; }
    }
}
