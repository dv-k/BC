using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class AreaData
    {
        public AreaData() {
            Area = new AreaPoint(0, 0, 0, 0, 0, 0);
        }
        public AreaData(AreaPoint area,  double realWidth, double realHeigh)
        {
            Area = area;
            RealHeigh = realHeigh;
            RealWidth = realWidth;
        }

        public AreaPoint Area { get; set; }

        public double RealHeigh { get; set; }
        public double RealWidth { get; set; }
    }
}
