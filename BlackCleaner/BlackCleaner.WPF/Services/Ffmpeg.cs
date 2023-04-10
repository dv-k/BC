using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BlackCleaner.WPF.Services
{
    public class Ffmpeg
    {
        public Ffmpeg() 
        { 

        }

        public async  Task<List<CropdetectInfo>> CropdetectAsync(string inputfile)
        {
            return await Task.Run(() => Cropdetect(inputfile));
        }
        public List<CropdetectInfo> Cropdetect(string inputfile)
        {
            DebugStart();
            var allLine =  string.Join("\n", Start($"-i \"{inputfile}\" -vf cropdetect,metadata=mode=print -f null -"));
            //\s?w:(\d?)\s?h:(\d?)\s?x:(\d?)\s?y:(\d?)\s?prs:(\d?)\s?t:([\d.]?)]
            //
            Regex regex = new Regex(@"x1:(\d+)\s*x2:(\d+)\s*y1:(\d+)\s*y2:(\d+)[\s\dxyhwpts:]*t:([\d.]+)");
            MatchCollection matches = regex.Matches(allLine);
            List<CropdetectInfo> result = new List<CropdetectInfo>();
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                result.Add(new CropdetectInfo(TimeSpan.Parse(match.Groups[5].Value.Replace('.', ':')),
                long.Parse(match.Groups[1].Value),
                long.Parse(match.Groups[2].Value),
                long.Parse(match.Groups[3].Value),
                long.Parse(match.Groups[4].Value)));
            }

            return result;



        }
        public async Task<bool> SnapshotAsync(string inputfile, string outputfile, TimeSpan time)
        {
            return await Task.Run(() => Snapshot(inputfile, outputfile, time));
        }
        public bool Snapshot(string inputfile, string outputfile, TimeSpan time)
        {
            DebugStart();
            Start($"-i \"{inputfile}\" -ss {time} -frames:v 1 \"{outputfile}\"");
            return File.Exists(outputfile);
        }
        private List<string> Start(string arg)
        {

            using (Process build = new Process())
            {

                build.StartInfo.Arguments = arg;
                build.StartInfo.FileName = Path.Combine("ffmpeg", "ffmpeg.exe");

                build.StartInfo.UseShellExecute = false;
                build.StartInfo.RedirectStandardOutput = true;
                build.StartInfo.RedirectStandardError = true;
                build.StartInfo.CreateNoWindow = true;
                build.EnableRaisingEvents = true;

                var output = new List<string>();
   
                build.ErrorDataReceived += DR;
                build.OutputDataReceived += DR;
                void DR(object sender, DataReceivedEventArgs e) 
                    {
                        Debug.WriteLine(e.Data);
                        output.Add(e.Data);
                    };
              
                build.Start();
                build.BeginErrorReadLine();
                build.BeginOutputReadLine();
                build.WaitForExit();

              
           
                return output;
            }
        }


        private void DebugStart([CallerMemberName] string arg = "N/A")
        {
            Debug.WriteLine($"Start '{arg}'...");
        }
    }
}
