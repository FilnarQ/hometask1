using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hometask1
{
    public class MetaLogger
    {
        private static int _files = 0;
        private static int _lines = 0;
        private static int _errors = 0;
        private static List<string> _badFiles = new();
        public MetaLogger()
        {
            
        }
        public void fileParsed(string name, int lines, int errors)
        {
            _files++;
            _lines += lines;
            if (errors!=0)
            {
                _errors += errors;
                _badFiles.Add(name);
            }  
        }
        public TimerCallback log()
        {
            Console.WriteLine($"files: {_files}, lines: {_lines}, errors: {_errors}");
            return null;
        }
        public async Task checkout(string path)
        {
            await using (StreamWriter stream = File.CreateText(path + "\\meta.log"))
            {
                string data = $"parsed_files: {_files}\nparsed_lines: {_lines}\nerrors: {_errors}\ninvalid_files: [{string.Join(", ",_badFiles)}]";
                await stream.WriteAsync(data);
                await stream.DisposeAsync();
            }
            _files = _lines = _errors = 0;
            _badFiles.Clear();
            return;
        }
    }
}
