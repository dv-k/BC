using BlackCleaner.WPF.Model;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace BlackCleaner.WPF.ViewModels
{
    /// <summary>
    /// Shell ViewModel
    /// </summary>
    public class ShellViewModel : BindableBase
    {
  

        public ShellViewModel()
        {
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
        private IMediaInfo _mediaInfo ;
        public IMediaInfo MediaInfo { get => _mediaInfo; private set => SetProperty(ref _mediaInfo, value, MediaInfoChanged); }

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
             await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);

            MediaInfo = await FFmpeg.GetMediaInfo(PathFile);
          

            await LoadPreviews();
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
