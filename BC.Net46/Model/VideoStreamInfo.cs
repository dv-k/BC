using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Net46.Model
{
    public class VideoStreamInfo : StreamInfoBase
    {
        public VideoStreamInfo(int index, CodecInfoBase codec , TimeSpan duration,double width, double height) : base(index, codec, duration)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}
