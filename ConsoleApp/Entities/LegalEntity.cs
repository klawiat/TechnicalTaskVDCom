using ConsoleApp.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Entities
{
    internal class LegalEntity:EntityBase
    {
        public int Id { get; set; }
        public string Inn { get; set; }
        public string Ogrn {  get; set; }
    }
}
