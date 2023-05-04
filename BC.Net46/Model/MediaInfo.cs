using BC.Net46.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class MediaInfo
    {
        public MediaInfo(List<AudioStreamInfo> audioStreams, List<VideoStreamInfo> videoStreams)
        {
            AudioStreams = audioStreams;
            VideoStreams = videoStreams;

        }

        public TimeSpan Duration { get => VideoStreams.Count > 0 ? VideoStreams.Max(x => x.Duration) : AudioStreams.Count > 0 ? AudioStreams.Max(x => x.Duration) : new TimeSpan(0); }
        public List<AudioStreamInfo> AudioStreams { get;}
        public List<VideoStreamInfo> VideoStreams { get; }
    }
}
