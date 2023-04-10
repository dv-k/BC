
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlackCleaner.WPF.Model
{
    public class ScreenshotInfo
    {
        public ScreenshotInfo(string fileName, TimeSpan timestamp)
        {
            FileName = fileName;
            Timestamp = timestamp;
        }

        public string FileName { get; }
        public TimeSpan Timestamp { get; }


    }
}
