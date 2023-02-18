using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hometask1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly FileWatcher _watcher;
        private readonly IOptions<Config> _options;

        public Worker(ILogger<Worker> logger, FileWatcher watcher, IOptions<Config> options)
        {
            _logger = logger;
            _watcher = watcher;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_options.Value.inputFolder == "ENTER_PATH" || _options.Value.outputFolder == "ENTER_PATH")
            {
                Console.WriteLine("Enter paths in config file");
                return;
            }
            Console.WriteLine("Write reset to get meta.log and restart file watcher (next files may start overwriting previous)\nSpecify logging level and input/output path in appsettings.json\nPress CTRL+C to stop");
            _watcher.Start(_options.Value.inputFolder, _options.Value.outputFolder);
            while (!stoppingToken.IsCancellationRequested)
            {
                if(Console.ReadLine() == "restart")
                {
                    _watcher.metaLog();
                    _watcher.resetWatcher();
                }
            }
        }
    }
}
