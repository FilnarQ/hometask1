using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hometask1
{
    class FileProcessor
    {
        ILogger<FileProcessor> _logger;
        public FileProcessor(ILogger<FileProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessFile(string outPath, string path, int counter, IServiceProvider serviceProvider)
        {
            if (!File.Exists(path)) return;
            _logger.LogInformation($"processing {path}");
            List<Transaction> list = list = await readFile(path);
            string json = serialize(list);
            using (var scope = serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<MetaLogger>();
                var errors = from line in list where line.valid == false select line.valid;
                logger.fileParsed(path, list.Count, errors.Count());
            }
            await writeFile(json, outPath + "\\output" + counter +".json");
            _logger.LogInformation($"finished {path}");
            return;
        }

        public async Task<List<Transaction>> readFile(string path)
        {
            _logger.LogInformation($"reading {path}");
            List<Transaction> list = new();
            using (StreamReader sr = File.OpenText(path))
            {
                string s = null;
                if (path.Substring(path.Length - 3, 3) == "csv") await sr.ReadLineAsync();
                while ((s = await sr.ReadLineAsync()) != null)
                {
                    list.Add(new Transaction(s));
                }
            }
            return list;
        }

        public string serialize(List<Transaction> list)
        {
            List<City> cityList = new List<City> { };
            var byCity = from line in list where line.valid==true group line by line.city;
            foreach(var city in byCity)
            {
                City tempCity = new City();
                tempCity.city = (from line in city select line.city).First();
                tempCity.total = (from line in city select line.payment).Sum();
                tempCity.services = new List<Service> { };
                var byService = from line in city group line by line.service;
                foreach(var service in byService)
                {
                    Service tempService = new Service();
                    tempService.name = (from line in service select line.service).First();
                    tempService.total = (from line in service select line.payment).Sum();
                    tempService.payers = new List<Payer> { };
/*                    tempService.payers = (from line in service select new Payer(line));*/
                    foreach(var line in service)
                    {
                        tempService.payers.Add(new Payer(line));
                    }
                    tempCity.services.Add(tempService);
                }
                cityList.Add(tempCity);
            }
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string data = JsonSerializer.Serialize(cityList, options);
/*            _logger.LogInformation(data);*/
            return data;
        }

        public async Task writeFile(string data, string path)
        {
            await using (StreamWriter stream = File.CreateText(path))
            {
                await stream.WriteAsync(data);
                await stream.DisposeAsync();
            }
            return;
        }
    }
}
