using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Services
{
    public class Ffmpeg
    {
        public Ffmpeg() 
        { 

        }

        public bool Snapshot(string inputfile, string outputfile, TimeSpan time)
        {
            Start($"-i {inputfile} -ss {time} -frames:v 1 {outputfile}");
        }

        private List<string> Start(string arg)
        {

            using (Process build = new Process())
            {

                build.StartInfo.Arguments = arg;
                build.StartInfo.FileName = "ffmpeg.exe";

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
                build.WaitForExit(); ;
                return output;
            }
        }

    }
}
