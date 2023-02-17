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

        /*public City(IEnumerable<Transaction> list)
        {
            city = list.ElementAt(0).city;
            total = (from line in list select line.payment).Sum();
            List<Service> servicesList = new();
            var services = (from line in list select line.service).Distinct();
            foreach(string service in services)
            {
                servicesList.Add(new Service(from line in list where line.service == service select line));
            }
        }*/
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
