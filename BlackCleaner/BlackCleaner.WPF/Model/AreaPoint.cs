using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class AreaPoint
    {
        public AreaPoint(double x1, double y1, double x2, double y2, double w, double h)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            W = w;
            H = h;
        }

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public double H { get; set; }
        public double W { get; set; }
    }
}
