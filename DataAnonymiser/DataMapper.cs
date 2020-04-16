using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DataAnonymiser
{
    public class DataMapper
    {
        public Dictionary<string, string> Names { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ContactNumbers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> EmailAddresses { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> NearestFroms { get; set; }
        public Dictionary<string, string> NearestTos { get; set; }
        public Dictionary<string, string> IpAddresses { get; set; }
        public int TotalCount { get; set; }

        public string GetIpAddress()
        {
            var data = new byte[4];
            new Random().NextBytes(data);
            IPAddress ip = new IPAddress(data);
            return ip.ToString();
        }

        public List<MoverDisqualificationModel> GetData()
        {
            var options = new DbContextOptions<DataAccessor>();
            var dbContext = new DataAccessor("Connection String", options);
            var data = dbContext.MoverDisqualificationModels.FromSqlRaw(Query).ToList();
            TotalCount = data.Count;
            Names = data.Select(c => c.LastName.ToLower().Trim()).Distinct().Select(a => new KeyValuePair<string, string>(a, Faker.Name.FullName())).ToDictionary(b => b.Key, b => b.Value);
            Console.WriteLine("Processed Names");
            ContactNumbers = data.Select(c => c.ContactNumber.Trim()).Distinct().Select(a => new KeyValuePair<string, string>(a, Faker.Phone.Number())).ToDictionary(b => b.Key, b => b.Value);
            Console.WriteLine("Processed Numbers");
            NearestFroms = data.Select(c => c.NearestFrom.Trim()).Distinct().Select(a => new KeyValuePair<string, string>(a, Faker.Address.StreetAddress())).ToDictionary(b => b.Key, b => b.Value);
            NearestTos = data.Select(c => c.NearestTo.Trim()).Distinct().Select(a => new KeyValuePair<string, string>(a, Faker.Address.StreetAddress())).ToDictionary(b => b.Key, b => b.Value);
            Console.WriteLine("Processed Addresses From and To");
            IpAddresses = data.Select(c => c.IPAddress.Trim()).Distinct().Select(a => new KeyValuePair<string, string>(a, GetIpAddress())).ToDictionary(b => b.Key, b => b.Value);
            Console.WriteLine("Processed IP's");
            data.ForEach(c => AddEmail(c.Domain, c.LastName.Trim().ToLower(), c.Email));
            Console.WriteLine("Processed Emails");
            //EmailAddresses = emails.Select(a => new KeyValuePair<string, string>(a.Email, GetEmail(a.LastName.ToLower().Trim(), a.Domain))).Distinct().ToDictionary(b => b.Key, b => b.Value);

            //foreach (var d in data)
            //{
            //    AddMapper(nameof(MoverDisqualificationModel.LastName), d.LastName);
            //}
            Console.WriteLine(data.Count);
            return MapData(data);
        }

        public List<MoverDisqualificationModel> MapData(List<MoverDisqualificationModel> data)
        {
            List<MoverDisqualificationModel> models = new List<MoverDisqualificationModel>();

            foreach (var d in data)
            {
                models.Add(MapModel(d));
            }
            return models;

        }

        public MoverDisqualificationModel MapModel(MoverDisqualificationModel model)
        {
            return new MoverDisqualificationModel()
            {
                ConfirmationDate = model.ConfirmationDate
                ,
                ContactNumber = ContactNumbers[model.ContactNumber.Trim()],
                Domain = model.Domain,
                Email = EmailAddresses[model.Email],
                IPAddress = IpAddresses[model.IPAddress],
                IsDisqualifiedMover = model.IsDisqualifiedMover,
                LastName = Names[model.LastName.ToLower().Trim()],
                MoverId = model.MoverId + 9838,
                NearestFrom = NearestFroms[model.NearestFrom.Trim()],
                NearestTo = NearestTos[model.NearestTo.Trim()],
                RegistrationDate = model.RegistrationDate,
                ServiceId = model.ServiceId
            };


        }

        public string GetEmail(string lastName, string domain)
        {
            var name = Names[lastName];
            return name.Replace(" ", "") + "@" + domain;
        }

        public int Count { get; set; } = 0;

        public void AddEmail(string domain, string lastname, string email)
        {
            Console.WriteLine($"processing {Count}/{TotalCount}");
            if (!CheckDictionaryForExistingKey(EmailAddresses, email))
            {
                EmailAddresses.Add(email, GetEmail(lastname, domain));
            }
            Count++;
        }

        public bool CheckDictionaryForExistingKey(Dictionary<string, string> dict, string input)
        {
            return dict.Any(a => a.Key == input);
        }


        public void AddMapper(string listName, string input)
        {
            switch (listName)
            {
                case nameof(MoverDisqualificationModel.Email):
                    if (!CheckDictionaryForExistingKey(Names, input))
                        Names.Add(input, Faker.Name.FullName());
                    break;
            }
        }

        public void MapFirstName(MoverDisqualificationModel moverDisqualificationModel)
        {

        }

        public const string Query = @"Secret Query";
    }
}
