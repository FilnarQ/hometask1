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
    public class FileWatcher
    {
        private string _folder = Path.Join(Environment.CurrentDirectory, "rawData");
        private string[] _filter = { "*.txt", "*.csv" };
        FileSystemWatcher _fileSystemWatcher;
        ILogger<FileWatcher> _logger;
        IServiceProvider _serviceProvider;

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
        }

        private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var processorService = scope.ServiceProvider.GetRequiredService<FileProcessor>();
                Task.Run(async () =>
                {
                    await processorService.ProcessFile(e.FullPath);
                    return;
                });
            }
        }
    }
}
