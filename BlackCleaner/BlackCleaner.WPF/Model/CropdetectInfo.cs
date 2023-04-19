using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class CropdetectInfo : AreaPoint
    {
        public CropdetectInfo(CropdetectInfo selectedCropdetectInfo) : base(selectedCropdetectInfo.X1, selectedCropdetectInfo.Y1,
             selectedCropdetectInfo.X2, selectedCropdetectInfo.Y2, selectedCropdetectInfo.W, selectedCropdetectInfo.H)
        {
            this.Time= selectedCropdetectInfo.Time;

        }

        public CropdetectInfo(TimeSpan time, double x1, double x2, double y1, double y2, double w, double h) : base(x1, y1, x2, y2, w, h)
        {
            Time = time;
        }

        public TimeSpan Time { get; }
    
    }
}
