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
        public FileProcessor(ILogger<FileProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessFile(string path)
        {
            if (!File.Exists(path)) return;
            _logger.LogInformation($"processing {path}");
            List<Transaction> list = list = await readFile(path);
            _logger.LogInformation($"finished {path}");
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
                    _logger.LogInformation($"{list.Last().valid}");
                }
            }
            return list;
        }

        /*public async Task<List<int>> modifyFile(List<Transaction> list)
        {
            
        }*/
    }
}
