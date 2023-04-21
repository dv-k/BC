using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class MediaInfo
    {
        public MediaInfo(TimeSpan duration, double width, double height)
        {
            Duration = duration;
            Width = width;
            Height = height;
        }

        public TimeSpan Duration { get;}
        public double Width { get;}
        public double Height { get;}
    }
}
