using System.Diagnostics;
using System.Resources;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

Console.WriteLine("Проверяем ffmpeg...");
await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
Console.WriteLine("Проверяем ffmpeg...");

string inputFileName = "C:\\Users\\tkach\\OneDrive\\Документы\\GitHub\\BlackCleaner\\Video\\s\\sample-640x360_b0mo7oTO.m2ts";
var info = await FFmpeg.GetMediaInfo(inputFileName);
Console.WriteLine("Продолжительность: " + info.Duration);
string output = Path.Combine("C:\\Users\\tkach\\OneDrive\\Документы\\GitHub\\BlackCleaner\\Video\\s", Guid.NewGuid() + ".mp4");
string output11 = Path.Combine("C:\\Users\\tkach\\OneDrive\\Документы\\GitHub\\BlackCleaner\\Video\\s", Guid.NewGuid() + ".png");

//var snippet = await FFmpeg.Conversions.FromSnippet.Convert(, output);

IConversion result = await FFmpeg.Conversions.FromSnippet.Snapshot(inputFileName, output11, new TimeSpan(1));
await result.Start();
Console.WriteLine(result.OutputFilePath);