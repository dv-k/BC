using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class CropdetectInfo
    {
        public CropdetectInfo(CropdetectInfo selectedCropdetectInfo)
        {
            this.X1= selectedCropdetectInfo.X1;
            this.Y1= selectedCropdetectInfo.Y1;
            this.X2= selectedCropdetectInfo.X2;
            this.Y2= selectedCropdetectInfo.Y2;
            this.Time= selectedCropdetectInfo.Time;
            this.W = selectedCropdetectInfo.W;
            this.H = selectedCropdetectInfo.H;
        }

        public CropdetectInfo(TimeSpan time, double x1, double x2, double y1, double y2, double w, double h)
        {
            Time = time;
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
            H = h;
            W = w;
        }

        public TimeSpan Time { get; }
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public double H { get; set; }
        public double W { get; set; }
    }
}
