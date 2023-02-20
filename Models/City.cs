using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hometask1
{
    public class City
    {
        public string city { get; set; }
        public List<Service> services { get; set; }
        public double total { get; set; }

        public City(IGrouping<string, Transaction> list)
        {
            city = (from line in list select line.city).First();
            total = (from line in list select line.payment).Sum();
            services = new List<Service> { };
            var byService = from line in list group line by line.service;
            foreach(var service in byService)
            {
                Service tempService = new Service();
                tempService.name = (from line in service select line.service).First();
                tempService.total = (from line in service select line.payment).Sum();
                tempService.payers = new List<Payer> { };
                foreach(var line in service)
                {
                    tempService.payers.Add(new Payer(line));
                }
            services.Add(tempService);
            }
        }
    }
    
    public class Service
    {
        public string name { get; set; }
        public List<Payer> payers { get; set; }
        public double total { get; set; }
        /*internal Service(IEnumerable<Transaction> list)
        {
            name = list.ElementAt(0).service;
            total = (from line in list select line.payment).Sum();
            List<Payer>
        }*/
    }

    public class Payer
    {
        public string name { get; set; }
        public double payment { get; set; }
        public DateTime date { get; set; }
        public long account_number { get; set; }
        public Payer(Transaction t)
        {
            name = t.first_name + " " + t.last_name;
            payment = t.payment;
            date = t.date;
            account_number = t.account_number;
        }
    }
}
