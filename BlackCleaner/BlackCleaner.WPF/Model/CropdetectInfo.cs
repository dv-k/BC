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
        public CropdetectInfo(TimeSpan time, long x1, long x2, long y1, long y2)
        {
            Time = time;
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public TimeSpan Time { get; }
        public Int64 X1 { get; }
        public Int64 X2 { get;  }
        public Int64 Y1 { get; }
        public Int64 Y2 { get;  }
    }
}
