using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class CropdetectInfo : СropArea
    {
        public CropdetectInfo(CropdetectInfo selectedCropdetectInfo) : base(selectedCropdetectInfo.X1, selectedCropdetectInfo.Y1,
             selectedCropdetectInfo.X2, selectedCropdetectInfo.Y2)
        {

            this.Time= selectedCropdetectInfo.Time;

        }

        public CropdetectInfo(TimeSpan time, double x1, double x2, double y1, double y2) : base(x1, y1, x2, y2)
        {

            Time = time;
        }

        public TimeSpan Time { get; }


    }

    class CropdetectInfoComparer : IEqualityComparer<CropdetectInfo>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(CropdetectInfo x, CropdetectInfo y)
        {

            return x.X1 == y.X1 && y.Y1 == x.Y1 && x.X2 == y.X2 && x.Y2 == y.Y2;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(CropdetectInfo product)
        {
            return 0;
        }
    }
}
