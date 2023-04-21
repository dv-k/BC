using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class СropArea
    {
        public СropArea(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

      

        public override bool Equals(object obj)
        {
            return obj is СropArea point &&
                   X1 == point.X1 &&
                   X2 == point.X2 &&
                   Y1 == point.Y1 &&
                   Y2 == point.Y2;
        }
    }
}
