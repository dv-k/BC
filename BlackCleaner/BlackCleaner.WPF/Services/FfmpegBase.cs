using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Services
{
    public abstract class FfmpegBase
    {
        public abstract List<string> Start(string arg);
        public List<string> Start(string app, string arg)
        {

            using (Process build = new Process())
            {

                build.StartInfo.Arguments = arg;
                build.StartInfo.FileName = app ;
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
                    if (e.Data != null)
                    {
                        string newData = e.Data.Trim();
                        Debug.WriteLine(newData);
                           output.Add(newData);
                    }

                };

                build.Start();
                build.BeginErrorReadLine();
                build.BeginOutputReadLine();
                build.WaitForExit();

                Debug.WriteLine("ExitCode: " + build.ExitCode);
                return output;
            }
        }


        internal void DebugStart([CallerMemberName] string arg = "N/A")
        {
            Debug.WriteLine($"Start '{arg}'...");
        }


    }
}
