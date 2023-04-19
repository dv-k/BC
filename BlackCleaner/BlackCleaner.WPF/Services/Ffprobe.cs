using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Services
{
    public class Ffprobe : FfmpegBase
    {
        public Ffprobe()
        {

        }
        public MediaInfo GetMediaInfo (string inputfile)
        {
            DebugStart();


            DebugStart();
            var allLine = string.Join(" ", Start($"-i {inputfile}"));

            Regex regex = new Regex(@"Duration:\s*([\d:.]*)[\d\D]*Stream*[\d\D]*,\s(\d+)x(\d+)");
            MatchCollection matches = regex.Matches(allLine);
        

            if(matches.Count > 0) 
            {
                var match = matches[0];
                return new MediaInfo(TimeSpan.Parse(match.Groups[1].Value), Double.Parse(match.Groups[2].Value), Double.Parse(match.Groups[3].Value));
            }
            return null;




        }

        public override List<string> Start(string arg)
        {
            return this.Start(Path.Combine("ffmpeg", "ffprobe.exe"), arg);
        }
    }
}
