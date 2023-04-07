using BlackCleaner.WPF.Model;
using BlackCleaner.WPF.Services;
using FFmpegArgs;
using FFmpegArgs.Cores;
using FFmpegArgs.Cores.Inputs;
using FFmpegArgs.Executes;
using FFmpegArgs.Inputs;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;


namespace BlackCleaner.WPF.ViewModels
{
    /// <summary>
    /// Shell ViewModel
    /// </summary>
    public class ShellViewModel : BindableBase
    {
        private readonly Ffprobe _ffprobe;

        public ShellViewModel(Ffprobe ffprobe)
        {
            _ffprobe = ffprobe;
            OpenFileCommand = new DelegateCommand<object>(OpenFileCommandAction, CanOpenFileCommandAction);

        
         
        }
        #region property Status
        private string _status = "...";
        public string Status { get => _status; set => SetProperty(ref _status, value, StatusChanged); }

        private void StatusChanged()
        {

        }
        #endregion
        #region property CountPreviews
        private int _countPreviews = 5;
        public int CountPreviews { get => _countPreviews; set => SetProperty(ref _countPreviews, value, CountPreviewsChanged); }

        private async void CountPreviewsChanged()
        {
           await LoadPreviews();
        }
        #endregion
        #region property PathFile
        private string _pathFile = "...";
        public string PathFile { get => _pathFile; set => SetProperty(ref _pathFile, value, PathFileChanged); }

        private void PathFileChanged()
        {
           
        }
        #endregion


        #region property Previews
        private List<ScreenshotInfo> _previews = new List<ScreenshotInfo>();
        public List<ScreenshotInfo> Previews { get => _previews; set => SetProperty(ref _previews, value, PreviewsChanged); }

        private void PreviewsChanged()
        {

        }
        #endregion

        #region property MediaInfo
        private MediaInfo _mediaInfo ;
        public MediaInfo MediaInfo { get => _mediaInfo; private set => SetProperty(ref _mediaInfo, value, MediaInfoChanged); }

        private void MediaInfoChanged()
        {

        }
        #endregion


        #region conmmand OpenFileCommand 
        public DelegateCommand<object> OpenFileCommand { get; private set; }


        void OpenFileCommandAction(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.m2ts|*.m2ts|*.*|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PathFile = openFileDialog.FileName;
                LoadFile();
            }

        }

        bool CanOpenFileCommandAction(object parameter)
        {
            return true;
        }
        #endregion


        async void LoadFile()
        {
            Status = "Подождите...";

            this.MediaInfo =  _ffprobe.GetMediaInfo(PathFile);

         /*   using (Process build = new Process())
            {
               // build.StartInfo.WorkingDirectory = @"dir";
                build.StartInfo.Arguments = $"-i \"{PathFile}\" -vf cropdetect,metadata=mode=print -f null -";
                build.StartInfo.FileName = "ffmpeg.exe";

                build.StartInfo.UseShellExecute = false;
                build.StartInfo.RedirectStandardOutput = true;
                build.StartInfo.RedirectStandardError = true;
                build.StartInfo.CreateNoWindow = true;
                build.ErrorDataReceived += build_ErrorDataReceived;
                build.OutputDataReceived += build_ErrorDataReceived;
                build.EnableRaisingEvents = true;
                build.Start();
                build.BeginOutputReadLine();
                build.BeginErrorReadLine();
                build.WaitForExit();
            }
         */










        }

        static void build_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //string strMessage =
            Debug.WriteLine( e.Data);

        }
        async Task LoadPreviews()
        {
            Status = "Загрузка превью..."; 

            long step = MediaInfo.Duration.Ticks / CountPreviews;
            long tinks = 0;

            List<ScreenshotInfo> previews = new List<ScreenshotInfo>();


            for(int i = 0; i < CountPreviews; i++)
            {
                TimeSpan ts ;
                if (CountPreviews == i + 1)
                    ts = new TimeSpan(tinks);
                else
                    ts = new TimeSpan(tinks);
                string fileName = Path.GetTempFileName() + ".png";
                previews.Add(new ScreenshotInfo(fileName, new TimeSpan(tinks)));
                tinks += step;
            }

          await   Parallel.ForEachAsync(previews, async (item, token)=>  await item.LoadFile(this.PathFile));


            Previews = previews;
            Status = "Готово!";
            //  Console.WriteLine("Продолжительность: " + info.Duration);
        }
    }
}
