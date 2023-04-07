using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Services
{
    public class Ffprobe
    {
        public Ffprobe()
        {

        }
        public MediaInfo GetMediaInfo (string inputfile)
        {
            var data = Start(inputfile, $"-i {inputfile}");
           var durationLine = data.Find((x) => x.StartsWith("Duration"));
            if (durationLine == null)
                throw new Exception("duration not found");

            Regex regex = new Regex(@"^Duration: ([\d:.]*),");
            MatchCollection matches = regex.Matches(durationLine);
            if (matches.Count > 0)
            {

                return new MediaInfo(TimeSpan.Parse(matches[0].Groups[1].Value));
;
            }
            else
            {
                throw new Exception("duration not found");
            }

          
           
        }

        private List<string>  Start(string inputfile, string arg)
        {

            using (Process build = new Process())
            {
              
                build.StartInfo.Arguments = arg;
                build.StartInfo.FileName = "ffprobe.exe";

                build.StartInfo.UseShellExecute = false;
                build.StartInfo.RedirectStandardOutput = true;
                build.StartInfo.RedirectStandardError = true;
                build.StartInfo.CreateNoWindow = true;
           
                build.EnableRaisingEvents = true;
                build.Start();
         
                var output = new List<string>();

                while (build.StandardOutput.Peek() > -1)
                {
                    output.Add(build.StandardOutput.ReadLine().Trim());
                }

                while (build.StandardError.Peek() > -1)
                {
                    output.Add(build.StandardError.ReadLine().Trim());
                }
                build.WaitForExit();;
                return output;
            }
        }
    }
}
