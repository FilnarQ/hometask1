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
                string config = "appsettings.json:\n\n{\n\t\"Logging\": {\n\t\t\"LogLevel\": {\n\t\t\t\"Default\": \"Information\",\n\t\t\t\"Microsoft\": \"Warning\",\n\t\t\t\"Microsoft.Hosting.Lifetime\": \"Warning\"\n\t\t}\n\t},\n\t\"Config\": {\n\t\t\"inputFolder\": \"ENTER_PATH\",\n\t\t\"outputFolder\": \"ENTER_PATH\"\n\t}\n}";
                Console.WriteLine("\n\tEnter paths in 'appsettings.json'\n\tDo not use backslashes(\"\\\") in path, use / instead");
                Console.WriteLine(config);
                return;
            }
            Console.WriteLine("\n\tWrite reset to get meta.log and restart file watcher\n\t(before restarting proceed to copy output files in safe place, or they may be owerwritten later)\n\tSpecify logging level and input/output path in appsettings.json\n\tPress CTRL+C to stop and exit\n");
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
