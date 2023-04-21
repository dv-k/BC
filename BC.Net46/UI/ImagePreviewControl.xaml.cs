using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        #region private
        int mouseMode = -1;
        bool resizeMode = false;
        int cornerOffset = 5;
        Action<Point, double, double> resizeAction;
        #endregion

        #region dependencyProperty ImageSourceProperty
        public static readonly DependencyProperty ImageSourceProperty =  DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePreviewControl), new PropertyMetadata(null, ImageSourcePropertyChangetCall));
        private static void ImageSourcePropertyChangetCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (ImagePreviewControl)d;
            self.image.Source = e.NewValue as ImageSource;
          //  self.SourceUpdated();

        }
        #endregion
        #region dependencyProperty AreaDataProperty
        public static readonly DependencyProperty AreaDataProperty = DependencyProperty.Register("AreaData", typeof(СropAreaData), typeof(ImagePreviewControl), new FrameworkPropertyMetadata(new СropAreaData(),  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, AreaDataChangetCall));
        private static void AreaDataChangetCall(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (ImagePreviewControl)d;
            var value = (СropAreaData)e.NewValue;
            if (self.IsLoaded)
                self.SourceUpdated();


        }

        #endregion
        #region property ImageSource
        public ImageSource ImageSource { get => (ImageSource)GetValue(ImageSourceProperty); set => SetValue(ImageSourceProperty, value); }
        #endregion
        #region property AreaData
        public СropAreaData AreaData { get => (СropAreaData)GetValue(AreaDataProperty); set => SetValue(AreaDataProperty, value); }
        #endregion

        public ImagePreviewControl()
        {
            InitializeComponent();
        }

        private void SourceUpdated()
        {
            if (AreaData != null)
            {
                Debug.WriteLine("Mar -- ");
                double ah, aw, kW, kH;
                kH = image.ActualHeight / AreaData.RealHeigh ;
                kW = image.ActualWidth / AreaData.RealWidth ;
                rectanglemain.Height = ah = image.ActualHeight;
                rectanglemain.Width =  aw = image.ActualWidth;
    
                if(!DesignerProperties.GetIsInDesignMode(this))
                {

                    Debug.WriteLine("Mar: " + AreaData.Area.X1 * kW);


                    rectangle.Margin = new Thickness(AreaData.Area.X1 * kW, AreaData.Area.Y1 * kH, aw - ((AreaData.Area.X2 + 1) * kW), ah - ((AreaData.Area.Y2 + 1) * kH));
                    rectangleBorder.Margin = new Thickness(AreaData.Area.X1 * kW - 2, AreaData.Area.Y1 * kH - 2, aw - ((AreaData.Area.X2 + 1) * kW) - 2, ah - ((AreaData.Area.Y2 + 1) * kH) - 2);

                }
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

        private void rectangleBorder_MouseMove(object sender, MouseEventArgs e)
        {
           
            Point pos = e.GetPosition(this.rectangleBorder);
            double aw = rectangleBorder.ActualWidth;
            double ah = rectangleBorder.ActualHeight;

            double kW, kH;
            kH = image.ActualHeight / AreaData.RealHeigh;
            kW = image.ActualWidth / AreaData.RealWidth;


            if (resizeMode == false)
            {
                if ((pos.X >= 0 && pos.X <= 2 || pos.X >= aw - 2 && pos.X <= aw) && pos.Y > cornerOffset && pos.Y < ah - cornerOffset)
                {
                    this.rectangleBorder.Cursor = Cursors.SizeWE;
                }

                else if ((pos.Y >= 0 && pos.Y <= 2 || pos.Y >= ah - 2 && pos.Y <= ah) && pos.X > cornerOffset && pos.X < aw - cornerOffset)
                {
                    this.rectangleBorder.Cursor = Cursors.SizeNS;

                }
                else if (pos.X >= 0 && pos.X <= cornerOffset && pos.Y >= 0 && pos.Y <= cornerOffset || pos.X >= aw - cornerOffset && pos.X <= aw && pos.Y >= ah - cornerOffset && pos.Y <= ah)
                {
                    this.rectangleBorder.Cursor = Cursors.SizeNWSE;

                }
                else if (pos.X >= aw - cornerOffset && pos.X <= aw && pos.Y >= 0 && pos.Y <= cornerOffset || pos.X >= 0 && pos.X <= cornerOffset && pos.Y >= ah - cornerOffset && pos.Y <= ah)
                {
                    this.rectangleBorder.Cursor = Cursors.SizeNESW;

                }
            }

         
     
  

        }

        private void rectangleBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                resizeMode = true;
                rectangleBorder.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#7F808000");



                Point pos = e.GetPosition(this.rectangleBorder);
                double aw = rectangleBorder.ActualWidth;
                double ah = rectangleBorder.ActualHeight;

                double kW, kH;
                kH = image.ActualHeight / AreaData.RealHeigh;
                kW = image.ActualWidth / AreaData.RealWidth;


                if (pos.X >= 0 && pos.X <= 2 && pos.Y > cornerOffset)
                {
                
                        resizeAction = ((x, kWx, kHx) =>
                        {
                            var nV = Math.Round( (x.X) / kWx);
                            if (nV < AreaData.Area.X2)
                            {
                                AreaData.Area.X1 = nV;
                            }
                            else
                            {
                                AreaData.Area.X2 = nV;
                            }   
                            AreaData = new СropAreaData(AreaData);
                           // SourceUpdated();


                        });
                }
                else if (pos.X >= aw - 2 && pos.X <= aw && pos.Y < ah - cornerOffset)
                {
                        resizeAction = ((x, kWx, kHx) =>
                        {
                            var nV = Math.Round((x.X) / kWx);
                            if (nV > AreaData.Area.X1)
                            {
                                AreaData.Area.X2 = nV;
                            }
                            else
                            {
                                AreaData.Area.X1 = nV;
                            }
                            AreaData = new СropAreaData(AreaData);
                        });

                }
                else if (pos.Y >= 0 && pos.Y <= 2 && pos.X > cornerOffset)
                {
                    resizeAction = ((x, kWx, kHx) =>
                    {
                        var nV = Math.Round((x.Y) / kHx);
                        if (nV < AreaData.Area.Y2)
                        {
                            AreaData.Area.Y1 = nV;
                        }
                        else
                        {
                            AreaData.Area.Y2 = nV;
                        }
                        AreaData = new СropAreaData(AreaData);
                    });

                }
                else if (pos.Y >= ah - 2 && pos.Y <= ah && pos.X < aw - cornerOffset)
                {
                    resizeAction = ((x, kWx, kHx) =>
                    {
                        var nV = Math.Round((x.Y) / kHx);
                        if (nV > AreaData.Area.Y1)
                        {
                            AreaData.Area.Y2 = nV;
                        }
                        else
                        {
                            AreaData.Area.Y1 = nV;
                        }
                        AreaData = new СropAreaData(AreaData);
                    });

                }


            }


        }

        private void rectangleBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {

                resizeMode = false;
                rectangleBorder.Background = null;
                resizeAction = null;
            }
        }

        private void rectanglemain_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(this.rectanglemain);
            double kW, kH;
            kH = image.ActualHeight / AreaData.RealHeigh;
            kW = image.ActualWidth / AreaData.RealWidth;

            if (resizeMode)
            {
               
                resizeAction?.Invoke(pos, kW, kH);
            }
        }
    }
}
