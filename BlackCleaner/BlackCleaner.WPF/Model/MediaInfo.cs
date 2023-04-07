using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackCleaner.WPF.Model
{
    public class MediaInfo
    {
        public MediaInfo(TimeSpan duration)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get;}
    }
}
