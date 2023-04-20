using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class СropAreaData
    {
        public СropAreaData(СropAreaData areaData)
        {
            this.Area = areaData.Area;
            this.RealWidth = areaData.RealWidth;
            this.RealHeigh = areaData.RealHeigh;
        }
        public СropAreaData() {
            Area = new СropArea(0, 0, 0, 0);
        }
        public СropAreaData(СropArea area,  double realWidth, double realHeigh)
        {
            Area = area;
            RealHeigh = realHeigh;
            RealWidth = realWidth;
        }

        public СropArea Area { get; set; }

        public double RealHeigh { get; set; }
        public double RealWidth { get; set; }

     
    }
}
