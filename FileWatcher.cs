using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Hometask1
{
    public class FileWatcher
    {
        private string _folder = Path.Join(Environment.CurrentDirectory, "rawData");
        private string[] _filter = { "*.txt", "*.csv" };
        private string outPath = Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
        private int fileCounter = 1;
        FileSystemWatcher _fileSystemWatcher;
        ILogger<FileWatcher> _logger;
        IServiceProvider _serviceProvider;
        Timer _timer = new Timer();

        public FileWatcher(ILogger<FileWatcher> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            if (!Directory.Exists(_folder)) Directory.CreateDirectory(_folder);
            _fileSystemWatcher = new FileSystemWatcher(_folder);
            foreach (string f in _filter)
            {
                _fileSystemWatcher.Filters.Add(f);
            }
            _serviceProvider = serviceProvider;
        }

        public void Start()
        {
            _fileSystemWatcher.Created += _fileSystemWatcher_Created;

            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.IncludeSubdirectories = true;

            _logger.LogInformation($"watcher started");

            Directory.CreateDirectory(outPath);

            _timer.Enabled = true;
            _timer.Interval = /*DateTime.Today.AddDays(1).Subtract(DateTime.Now).TotalSeconds * 1000*/30000;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            _logger.LogInformation("timer set");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _logger.LogInformation("tick");
            using (var scope = _serviceProvider.CreateScope())
            {
                var metaLogger = scope.ServiceProvider.GetRequiredService<MetaLogger>();
                Task.Run(async () =>
                {
                    metaLogger.log();
                    await metaLogger.checkout(outPath);
                    return;
                });
            }
            fileCounter = 1;
            outPath = Path.Combine(Environment.CurrentDirectory, DateTime.Now.ToString("MM-dd-yyyy"));
            Directory.CreateDirectory(outPath);
            if (_timer.Interval != /*24 * 60 * */60 * 1000)
            {
                _timer.Interval = /*24 * 60 * */60 * 1000;
            }
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var processorService = scope.ServiceProvider.GetRequiredService<FileProcessor>();
                Task.Run(async () =>
                {
                    await processorService.ProcessFile(outPath, e.FullPath, fileCounter++, _serviceProvider);
                    return;
                });
            }
        }
    }
}
