using System;
using System.Globalization;
using System.Linq;

namespace Hometask1
{
    public class Transaction
    {
        public string first_name;
        public string last_name;
        public string address;
        public string city;
        public double payment;
        public DateTime date;
        public long account_number;
        public string service;

        public bool valid;

        public Transaction(string data)
        {
            valid = true;
            string[] arr = data.Split(",");
            arr = arr.Select(s => s.Trim()).ToArray();
            if (arr.Length == 9)
            {
                try
                {
                    first_name = arr[0];
                    last_name = arr[1];
                    address = $"{arr[2].Substring(1)},{arr[3]},{arr[4][0..^1]}";
                    city = $"{arr[2].Substring(1)}";
                    payment = Convert.ToDouble(arr[5], CultureInfo.InvariantCulture);
                    date = DateTime.ParseExact(arr[6], "yyyy-dd-MM", CultureInfo.InvariantCulture);
                    account_number = Convert.ToInt64(arr[7], CultureInfo.InvariantCulture);
                    service = arr[8] ?? throw new Exception($"error in transaction from {date}");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
        }

        public string check()
        {
            return "okay";
        }
    }
}
