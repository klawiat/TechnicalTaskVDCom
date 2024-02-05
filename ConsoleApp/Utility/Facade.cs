using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace ConsoleApp.Utility
{
    public class Facade : IDisposable
    {
        private NpgsqlDataReader reader { get; set; }

        private readonly NpgsqlConnection connection;
        private NpgsqlCommand command;

        public Facade(string connectionString)
        {
            this.connection = new NpgsqlConnection(connectionString);
            this.connection.Open();
            this.command = this.connection.CreateCommand();
        }
        public void AmountOfContracts()
        {
            command.CommandText = "SELECT SUM(amount) as Contract_Amount FROM contract " +
                    "WHERE EXTRACT(year from datesigned)=EXTRACT(year from now()) AND " +
                    "status = 'Signed'";
            reader = command.ExecuteReader();
            if (!reader.Read())
                return;
            Console.WriteLine($"Cумма всех заключенных договоров за текущий год: {reader.GetDecimal(0)}");
            reader.Close();
        }

        public void AmountUnderRussianContracts()
        {
            command.CommandText = "SELECT SUM(contract.amount) as Contracts_Amount, legalentity.name as Name from contract " +
                                "LEFT JOIN legalentity ON contract.legalentityid = legalentity.id " +
                                "WHERE legalentity.country='Russia' AND contract.status = 'Signed' " +
                                "GROUP BY legalentity.name";
            reader = command.ExecuteReader();
            Console.WriteLine("\tКонтрагент\tСумма");
            while (reader.Read())
                Console.WriteLine($"\t{reader.GetString(1)}\t\t{reader.GetDecimal(0)}");
            reader.Close();
        }
        public void EmailOfAuthorizedPersons()
        {
            command.CommandText = "SELECT DISTINCT  individual.email FROM individual " +
                                "JOIN contract ON contract.individualid = individual.id " +
                                "WHERE date_part('day',now()-contract.datesigned)<31 AND contract.amount>40000";
            reader = command.ExecuteReader();
            Console.WriteLine("\tEmail");
            while (reader.Read())
                Console.WriteLine($"\t{reader.GetString(0)}");
            reader.Close();
        }
        public void ChangeTheStatusOfContracts()
        {
            command.CommandText = "UPDATE contract SET status = 'Terminated' " +
                                "FROM individual " +
                                "WHERE contract.individualid = individual.id " +
                                "AND contract.status = 'Signed' AND individual.age>=60";
            int count = command.ExecuteNonQuery();
            Console.WriteLine($"Готово. Изменение коснулось {count} полей");
        }
        public void CreateReport()
        {
            Console.WriteLine("Отчет будет создан в папке с программой!");
            command.CommandText = "SELECT name, lastname, patronymic, email, phonenumber, TO_CHAR(birthday::timestamp,'D/MM/YYYY') FROM individual " +
                                "JOIN contract ON individual.id = contract.individualid " +
                                "WHERE individual.city = 'Moscow' AND contract.status = 'Signed'";
            reader = command.ExecuteReader();
            List<JObject> report = new List<JObject>();
            while (reader.Read())
            {
                JObject line = new JObject(
                    new JProperty("fio", $"{reader.GetString(1)} {reader.GetString(0)} {reader.GetString(2)}"),
                    new JProperty("email", reader.GetString(3)),
                    new JProperty("phone", reader.GetString(4)),
                    new JProperty("birthday", reader.GetString(5))
                    );
                report.Add(line);
            }
            string fileName = $"Очёт от {DateTime.Now.ToString("d")}.json";
            if (File.Exists(fileName))
                File.Delete(fileName);
            using (FileStream fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    JsonSerializer serializer = JsonSerializer.Create();
                    serializer.Serialize(sw, report);
                }
            }
            Console.WriteLine("Отчет готов");
        }

        public void Dispose()
        {
            this.connection.Close();
        }
    }
}
