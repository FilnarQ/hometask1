using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<List<Transaction>> readFile()
        {
            return new List<Transaction>();
        }
    }
}
