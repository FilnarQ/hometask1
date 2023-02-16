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
            using (StreamReader sr = File.OpenText(path))
            {
                string? s = null;
                while((s = await sr.ReadLineAsync()) != null)
                {
                    _logger.LogInformation(s);
                }

            }
            _logger.LogInformation($"finished {path}");
        }
    }
}
