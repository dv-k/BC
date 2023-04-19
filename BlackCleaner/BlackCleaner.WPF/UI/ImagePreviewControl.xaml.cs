using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ImTools.ImMap;

namespace BlackCleaner.WPF.UI
{
    /// <summary>
    /// Логика взаимодействия для ImagePreviewControl.xaml
    /// </summary>
    public partial class ImagePreviewControl : UserControl
    {

        public static readonly DependencyProperty ImageSourceProperty =  DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePreviewControl), new PropertyMetadata(null, ImageSourcePropertyChangetCall));


        private static void ImageSourcePropertyChangetCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (ImagePreviewControl)d;
            self.image.Source = e.NewValue as ImageSource;
          //  self.SourceUpdated();

        }

        public static readonly DependencyProperty AreaDataProperty = DependencyProperty.Register("AreaData", typeof(AreaData), typeof(ImagePreviewControl), new PropertyMetadata(new AreaData(), AreaDataChangetCall));
        private static void AreaDataChangetCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (ImagePreviewControl)d;
            var value = (AreaData)e.NewValue;
        
           
        }

       



        public ImageSource ImageSource { get => (ImageSource)GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }
        public AreaData AreaData { get => (AreaData)GetValue(AreaDataProperty); set => SetValue(AreaDataProperty, value); }
    
        public ImagePreviewControl()
        {
            InitializeComponent();
        }

        private void SourceUpdated()
        {
            if (AreaData != null)
            {

                double ah, aw, kW, kH;
                kH = image.ActualHeight / AreaData.RealHeigh ;
                kW =  image.ActualWidth / AreaData.RealWidth ;
                rectanglemain.Height = ah = image.ActualHeight;
                rectanglemain.Width =  aw = image.ActualWidth;
    
               rectangle.Margin = new Thickness(AreaData.Area.X1 * kW, AreaData.Area.Y1 * kH, aw - ((AreaData.Area.X2 + 1) * kW), ah - ((AreaData.Area.Y2 + 1) * kH));
               
                //rectangle.Width = AreaData.RealWidth * _kW;
                //  rectangle.Height = AreaData.RealHeigh * _kH;

            }
        }



        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            SourceUpdated();
        }

        private void userControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SourceUpdated();
        }
    }
}
