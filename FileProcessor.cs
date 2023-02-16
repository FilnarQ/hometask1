using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hometask1
{
    class FileProcessor
    {
        ILogger<FileProcessor> _logger;
        IServiceProvider _serviceProvider;
        public FileProcessor(ILogger<FileProcessor> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task ProcessFile(string path)
        {
            if (!File.Exists(path)) return;
            _logger.LogInformation($"processing {path}");
            List<Transaction> list = list = await readFile(path);
            _logger.LogInformation($"finished {path}, {list.ElementAt(0).valid}");
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
            return list;
        }
    }
}
