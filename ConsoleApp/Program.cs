using ConsoleApp.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    internal class Program
    {
        static string connectionString = "Host=localhost;Database=TechnicalTaskVDCom;Username=postgres;Password=Klawiat1324";
        static void Main(string[] args)
        {
            Facade facade = new Facade(connectionString);
            char answer = '1';
            while (answer != '6') 
            {
                Console.WriteLine("Выберите действие:\n" +
                "1.\tВывести сумму всех заключенных договоров за текущий год.\n" +
                "2.\tВывести сумму заключенных договоров по каждому контрагенту из России.\n" +
                "3.\tВывести список e-mail уполномоченных лиц, заключивших договора за последние 30 дней, на сумму больше 40000.\n" +
                "4.\tИзменить статус договора на \"Расторгнут\" для физических лиц, у которых есть действующий договор, и возраст которых старше 60 лет включительно.\n" +
                "5.\tСоздать отчет (текстовый файл, например, в формате xml, xlsx, json) содержащий ФИО, e-mail, моб. телефон, дату рождения физ. лиц, у которых есть действующие договора по компаниям, расположенных в городе Москва.\n" +
                "6.\tВыход.");
                answer = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (answer)
                {
                    case '1':
                        facade.AmountOfContracts();
                        break;
                    case '2':
                        facade.AmountUnderRussianContracts();
                        break;
                    case '3':
                        facade.EmailOfAuthorizedPersons();
                        break;
                    case '4':
                        facade.ChangeTheStatusOfContracts();
                        break;
                    case '5':
                        facade.CreateReport();
                        break;
                    case '6':
                        facade.Dispose();
                        continue;
                    default:
                        Console.WriteLine("Нет такого действия");
                        break;
                }
                Console.WriteLine("\n");
            }

        }
    }
}
