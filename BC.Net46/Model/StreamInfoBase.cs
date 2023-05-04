using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC.Net46.Model
{
    public abstract class StreamInfoBase
    {
        public StreamInfoBase(int index, CodecInfoBase codec, TimeSpan duration)
        {
            Codec = codec;
            Index = index;
            Duration = duration;
        }

        public int Index { get; }
        public CodecInfoBase Codec { get; }

        public TimeSpan Duration { get; }
    }
}
