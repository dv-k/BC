using BlackCleaner.WPF.Model;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlackCleaner.WPF.ViewModels
{
    internal class ImagePreviewViewModel : BindableBase, IDialogAware
    {
        public ImagePreviewViewModel()
        {
            CloseCommand = new DelegateCommand<object>(CloseCommandAction, CanCloseCommandAction);
            SizeChangedCommand = new DelegateCommand<object>(SizeChangedCommandAction, CanSizeChangedCommandAction);
        }

        #region property CropdetectInfo
        private CropdetectInfo _cropdetectInfo = null;
        public CropdetectInfo CropdetectInfo { get => _cropdetectInfo; set => SetProperty(ref _cropdetectInfo, value, CropdetectInfoChanged); }

        private void CropdetectInfoChanged()
        {
           
        }
        #endregion

        #region property MediaInfo
        private MediaInfo _mediaInfo = null;
        public MediaInfo MediaInfo { get => _mediaInfo; set => SetProperty(ref _mediaInfo, value, MediaInfoChanged); }

        private void MediaInfoChanged()
        {

        }
        #endregion


        #region property Preview
        private ScreenshotInfo _preview = null;




        public ScreenshotInfo Preview { get => _preview; set => SetProperty(ref _preview, value, PreviewChanged); }

        private void PreviewChanged()
        {

        }
        #endregion

        #region property HeightPreview
        private AreaData _areaData;

        public AreaData AreaData { get => _areaData; set => SetProperty(ref _areaData, value, AreaDataChanged); }

        private void AreaDataChanged()
        {
         
        }
        #endregion
        #region property WidthPreview
        private double _widthPreview = Double.NaN;



        public double WidthPreview { get => _widthPreview; set => SetProperty(ref _widthPreview, value, WidthPreviewChanged); }

        private void WidthPreviewChanged()
        {
          
        }
        #endregion







        #region conmmand CloseCommand 
        public DelegateCommand<object> CloseCommand { get; private set; }


        void CloseCommandAction(object parameter)
        {
            DialogParameters dp = new DialogParameters();
            dp.Add("IP_CropdetectInfo", CropdetectInfo);

            RaiseRequestClose(new DialogResult(ButtonResult.OK, dp));
        }

        bool CanCloseCommandAction(object parameter)
        {
            return true;
        }
        #endregion


        #region conmmand SizeChangedCommand 
        public DelegateCommand<object> SizeChangedCommand { get; private set; }


        void SizeChangedCommandAction(object parameter)
        {
          
        }

        bool CanSizeChangedCommandAction(object parameter)
        {
            return true;
        }
        #endregion
        public string Title => "Предварительный просмотр";

        public event Action<IDialogResult> RequestClose;


        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
            
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            CropdetectInfo =  parameters.GetValue<CropdetectInfo>("IP_CropdetectInfo");
            MediaInfo = parameters.GetValue<MediaInfo>("IP_MediaInfo");
            Preview = parameters.GetValue<ScreenshotInfo>("IP_ScreenshotInfo");


            AreaData = new AreaData(CropdetectInfo, MediaInfo.Width, MediaInfo.Height);


        }

     
    }
}
