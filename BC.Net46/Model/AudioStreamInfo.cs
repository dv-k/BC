using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Net46.Model
{
    public class AudioStreamInfo : StreamInfoBase
    {
        public AudioStreamInfo(int index, CodecInfoBase codec, TimeSpan duration) : base(index,codec, duration)
        {
        }
    }
}
