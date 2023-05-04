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
    public class Ffmpeg : FfmpegBase
    {

        public Ffmpeg() : base(Path.Combine("ffmpeg", "ffmpeg.exe"))
        { 

        }

        public async  Task<List<CropdetectInfo>> CropdetectAsync(string inputfile)
        {
            return await Task.Run(() => Cropdetect(inputfile));
        }
        public List<CropdetectInfo> Cropdetect(string inputfile)
        {
            DebugStart();
            var allLine =  string.Join("\n", Start($" -i \"{inputfile}\" -vf cropdetect -f null -"));


       // Duration:\s * ([\d:.] *)[\d\D] * Stream *[\d\D] *,\s(\d +)x(\d +)
            Regex regex = new Regex(@"x1:(\d+)\s*x2:(\d+)\s*y1:(\d+)\s*y2:(\d+)[whxypts:\d\s]*\s+t:([\d.]+)");
            MatchCollection matches = regex.Matches(allLine);
            List<CropdetectInfo> result = new List<CropdetectInfo>();
            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                result.Add(new CropdetectInfo( TimeSpan.FromSeconds(Double.Parse(match.Groups[5].Value.Replace('.', ','))),
                double.Parse(match.Groups[1].Value),
                double.Parse(match.Groups[2].Value) + 1,
                double.Parse(match.Groups[3].Value) ,
                double.Parse(match.Groups[4].Value) + 1));
            }

            return result;



        }

        public async Task CroppingAsync(string inputfile, string outputfile, string сrop, string cv = "", string ca = "")
        {
             await Task.Run(() => Cropping(inputfile, outputfile, сrop, cv, ca));
        }

        public void Cropping(string inputfile, string outputfile, string сrop, string cv = "", string ca = "")
        {
            DebugStart();
            string cvT = string.Empty;
            string caT = string.Empty;
            if (!string.IsNullOrWhiteSpace(cv))
                cvT = "-c:v " + cv;
            if (!string.IsNullOrWhiteSpace(ca))
                caT = "-c:a " + ca;

            var allLine = string.Join("\n", Start($"-i {inputfile} -vf crop={сrop}   {cvT} {caT}    {outputfile} "));


        }
        public async Task<bool> SnapshotAsync(string inputfile, string outputfile, TimeSpan time)
        {
            return await Task.Run(() => Snapshot(inputfile, outputfile, time));
        }
        public bool Snapshot(string inputfile, string outputfile, TimeSpan time)
        {
            DebugStart();
            Start($" -loglevel error -i \"{inputfile}\" -ss {time} -frames:v 1  \"{outputfile}\"");
            return File.Exists(outputfile);
        }
     


    }
}
