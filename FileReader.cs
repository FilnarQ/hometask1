using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hometask1
{
    class FileReader
    {
        ILogger<FileReader> _logger;
        public FileReader(ILogger<FileReader> logger)
        {
            _logger = logger;
        }

        public async Task<List<Transaction>> readFile(string path)
        {
            _logger.LogInformation($"reading {path}");
            List<Transaction> list = new();
            using (StreamReader sr = File.OpenText(path))
            {
                string? s = null;
                while ((s = await sr.ReadLineAsync()) != null)
                {
                    list.Add(new Transaction(s));
                }
            }
            _logger.LogInformation($"finished reading {path}");
            return list;
        }
    }
}
