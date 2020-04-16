using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;

namespace DataAnonymiser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var dataMapper = new DataMapper();
                var data = dataMapper.GetData();
                using (var writer = new StreamWriter($"C:\\Users\\Taylor\\Desktop\\AIData{DateTime.Now.ToString("dd-MM-yyyy-HH-MM")}.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }

                Console.WriteLine(JsonConvert.SerializeObject(data));
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
            Console.WriteLine("Hello World!");
        }
    }
}
