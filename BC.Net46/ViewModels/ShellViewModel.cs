using BlackCleaner.WPF.Model;
using BlackCleaner.WPF.Services;

using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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
        private readonly Ffmpeg _ffmpeg;
        private readonly IDialogService _dialogService;

        public ShellViewModel(Ffprobe ffprobe, Ffmpeg ffmpeg, IDialogService dialogService)
        {
            _ffprobe = ffprobe;
            _ffmpeg= ffmpeg;
            _dialogService= dialogService;

            OpenFileCommand = new DelegateCommand<object>(OpenFileCommandAction, CanOpenFileCommandAction);
            StartCroppingCommand = new DelegateCommand<object>(StartCroppingCommandAction, CanStartCroppingCommandAction);


            if(!File.Exists(Path.Combine("ffmpeg", "ffmpeg.exe")) || !File.Exists(Path.Combine("ffmpeg", "ffprobe.exe")))
            {
                this.Status = "Нет файла ffmpeg.exe или ffprobe.exe в каталоге ffmpeg!";
            }
            else
            {
                this.Status = "Ожидание выбора файла";
            }


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
            LoadPreviews();
        }
        #endregion
        #region property PathFile
        private string _pathFile = "...";
        public string PathFile { get => _pathFile; set => SetProperty(ref _pathFile, value, PathFileChanged); }

        private void PathFileChanged()
        {
           
        }
        #endregion
        #region property PathFile
        private bool _isEnabled =  false;
        public bool IsEnabled { get => _isEnabled; set => SetProperty(ref _isEnabled, value, IsEnabledChanged); }

        private void IsEnabledChanged()
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

        #region property SelectedPreview
        private ScreenshotInfo _selectedPreview = null;
        public ScreenshotInfo SelectedPreview { get => _selectedPreview; set => SetProperty(ref _selectedPreview, value, SelectedPreviewChanged); }

        private void SelectedPreviewChanged()
        {
            if (_selectedPreview != null )
            {
                if(_selectedcropdetectInfo != null && _mediaInfo !=null)
                {
                    DialogParameters dp = new DialogParameters();
                    dp.Add("IP_CropdetectInfo", new CropdetectInfo(SelectedCropdetectInfo));
                    dp.Add("IP_ScreenshotInfo", SelectedPreview);
                    dp.Add("IP_MediaInfo", MediaInfo);
                    _dialogService.ShowDialog("ImagePreview", dp, x =>
                    {
                        var newCI = x.Parameters.GetValue<CropdetectInfo>("IP_CropdetectInfo");
                        if (newCI != null)
                        {
                            var r = this.CropdetectInfo.Find(y => y.X1 == newCI.X1 && y.Y1 == newCI.Y1 && y.X2 == newCI.X2 && y.Y2 == newCI.Y2);
                            if (r != null)
                            {
                                this.SelectedCropdetectInfo = r;
                            }
                            else
                            {
                                this.CropdetectInfo.Add(newCI);
                                this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CropdetectInfo)));
                                this.SelectedCropdetectInfo = newCI;
                            }
                        }
                        this.SelectedPreview = null;
                    });
                }
                else
                    this.SelectedPreview = null;

            }
        }
        #endregion

        #region property CropdetectInfo
        private List<CropdetectInfo> _cropdetectInfo = new List<CropdetectInfo>();
        public List<CropdetectInfo> CropdetectInfo { get => _cropdetectInfo; set => SetProperty(ref _cropdetectInfo, value, CropdetectInfoChanged); }

        private void CropdetectInfoChanged()
        {
            if (_cropdetectInfo.Count > 0)
                SelectedCropdetectInfo = _cropdetectInfo[0];
            else
                SelectedCropdetectInfo = null;
        }
        #endregion



        #region property SelectedCropdetectInfo
        private CropdetectInfo _selectedcropdetectInfo = null;
        public CropdetectInfo SelectedCropdetectInfo { get => _selectedcropdetectInfo; set => SetProperty(ref _selectedcropdetectInfo, value, SelectedCropdetectInfoChanged); }

        private void SelectedCropdetectInfoChanged()
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
            return File.Exists(Path.Combine("ffmpeg","ffmpeg.exe")) && File.Exists(Path.Combine("ffmpeg", "ffprobe.exe"));
        }
        #endregion

        #region conmmand OpenFileCommand 
        public DelegateCommand<object> StartCroppingCommand { get; private set; }


        async void StartCroppingCommandAction(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = Path.GetFileNameWithoutExtension(this.PathFile) + "-cropping" + Path.GetExtension(this.PathFile);

            if (saveFileDialog.ShowDialog() == true)
            {
                IsEnabled = false;
                Status = "Обработка...";
                if (saveFileDialog.OverwritePrompt)
                    File.Delete(saveFileDialog.FileName);
                string сrop = $"{(this._selectedcropdetectInfo.X2 - this._selectedcropdetectInfo.X1)}:{this._selectedcropdetectInfo.Y2 - this._selectedcropdetectInfo.Y1}:{this._selectedcropdetectInfo.X1}:{this._selectedcropdetectInfo.Y1}";
                await  _ffmpeg.CroppingAsync(this.PathFile, saveFileDialog.FileName, сrop, this.MediaInfo.VideoStreams.Count > 0 ? this.MediaInfo.VideoStreams[0].Codec.CodecName : String.Empty, "copy");
                Status = "Готово";
                IsEnabled = true;
            } 
        }

        bool CanStartCroppingCommandAction(object parameter)
        {
            return true;
        }
        #endregion

        async void LoadFile()
        {
            
            IsEnabled = false;
            Status = "Подождите...";

  

         this.MediaInfo = await _ffprobe.GetMediaInfoAsync(PathFile);
           await LoadPreviews();
            IsEnabled = false;
            Status = "Поиск области...";
            CropdetectInfo = (await _ffmpeg.CropdetectAsync(PathFile)).Distinct(new CropdetectInfoComparer()).ToList();
            Status = "Готово!";
            IsEnabled = true;
        }

        async Task LoadPreviews()
        {
            IsEnabled = false;
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


              Parallel.ForEach(previews, async (item, token) =>  _ffmpeg.Snapshot(PathFile, item.FileName, item.Timestamp));


            Previews = previews;
            Status = "Готово!";
            IsEnabled = true;
        }
    }
}
