using BC.Net46.Model;
using BlackCleaner.WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace BlackCleaner.WPF.Services
{
    public class Ffprobe : FfmpegBase
    {
        public Ffprobe() : base(Path.Combine("ffmpeg", "ffprobe.exe"))
        {

        }
        public async Task<MediaInfo> GetMediaInfoAsync(string inputfile)
        {
            return await Task.Run(() => GetMediaInfo(inputfile));
        }
        public MediaInfo GetMediaInfo (string inputfile)
        {
            DebugStart();

            var allLineStreams = string.Join("\n", Start($" -loglevel error -show_streams {inputfile}"));

            //Обрабатываем потоки
            Regex regexSreams = new Regex("\\[STREAM\\]\\s([\\d\\D]*?)\\s\\[\\/STREAM\\]", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            MatchCollection matchesSreams = regexSreams.Matches(allLineStreams);
            List<VideoStreamInfo> videoStreams = new List<VideoStreamInfo>();
            List<AudioStreamInfo> audioStreams = new List<AudioStreamInfo>();

            for (int i = 0; i < matchesSreams.Count; i++)
            {
                var matchesSream = matchesSreams[i];

                Regex regexSream = new Regex("^index=(.+)[\\d\\D]*^codec_name=(.+)[\\d\\D]*^codec_long_name=(.+)[\\d\\D]*^codec_type=(.+)[\\d\\D]*^duration=(.+)[\\d\\D]*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);

                MatchCollection matches1 = regexSream.Matches(matchesSream.Groups[0].Value);
                if (matches1.Count > 0)
                {
                    var m = matches1[0];
                    int index = int.Parse(m.Groups[1].Value);
                    CodecInfoBase codecInfoBase = new CodecInfoBase(m.Groups[2].Value, m.Groups[3].Value);
                    TimeSpan duration = TimeSpan.Zero;
                    if (Double.TryParse(m.Groups[5].Value.Replace('.', ','), out double d))
                    {
                        duration = TimeSpan.FromSeconds(d);
                    }
    


                    switch (m.Groups[4].Value.ToLower())
                    {
                        case "audio":
                            audioStreams.Add(new AudioStreamInfo(index, codecInfoBase, duration));
                            break;
                        case "video":
                            Regex regexVideo = new Regex("^width=(.+)[\\d\\D]*^height=(.+)[\\d\\D]", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
                            MatchCollection matchesVideo = regexVideo.Matches(matchesSream.Groups[1].Value);
                            double wVideo = 0;
                            double hVideo = 0;
                            if (matchesVideo.Count > 0)
                            {
                                wVideo = double.Parse(matchesVideo[0].Groups[1].Value);
                                hVideo = double.Parse(matchesVideo[0].Groups[2].Value);
                            }
                            videoStreams.Add(new VideoStreamInfo(index, codecInfoBase, duration, wVideo, hVideo));
                            break;
                    }
                }
            }

            return new MediaInfo(audioStreams, videoStreams);

        }
    }
}
