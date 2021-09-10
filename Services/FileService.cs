using Microsoft.WindowsAPICodePack.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeslaCamTheater.Exceptions;
using TeslaCamTheater.Models;

namespace TeslaCamTheater.Services
{
    public static class FileService
    {
        private const string RECENT_CLIPS = "RecentClips";
        private const string SAVED_CLIPS = "SavedClips";
        private const string SENTRY_CLIPS = "SentryClips";

        private const string EXT = ".mp4";
        private const string FRONT_SUFFIX = "-front" + EXT;
        private const string REAR_SUFFIX = "-back" + EXT;
        private const string LEFT_RPTR_SUFFIX = "-left_repeater" + EXT;
        private const string RIGHT_RPTR_SUFFIX = "-right_repeater" + EXT;

        private const int FILE_LENGTH_SKIP_THRESHOLD = 4096;

        /// <summary>
        /// Determines if this is a TeslaCam folder or not.
        /// </summary>
        public static void IsTeslaCamFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ErrorMessageException($"The selected folder (\"{path}\") does not exist or is not readable.");
            }

            if (!Directory.Exists(Path.Combine(path, RECENT_CLIPS))
                && !Directory.Exists(Path.Combine(path, SAVED_CLIPS))
                && !Directory.Exists(Path.Combine(path, SENTRY_CLIPS)))
            {
                throw new ErrorMessageException($"This folder (\"{path}\") does not appear to be a TeslaCam folder.\r\n\r\n(Select the folder called \"TeslaCam\", not a folder above or below it.)");
            }
            var di = new DirectoryInfo(path);
            if (!di.EnumerateFiles("*.mp4", SearchOption.AllDirectories).Any())
            {
                throw new ErrorMessageException($"This folder (\"{path}\") does not appear to have any video files in it!");
            }
        }

        public static async Task<List<Event>> LoadEvents(string path)
        {
            var events = new List<Event>();
            if (Directory.Exists(Path.Combine(path, RECENT_CLIPS)))
            {
                foreach (var evd in Directory.GetDirectories(Path.Combine(path, RECENT_CLIPS)))
                {
                    events.Add(await LoadEvent(evd));
                }
            }
            if (Directory.Exists(Path.Combine(path, SAVED_CLIPS)))
            {
                foreach (var evd in Directory.GetDirectories(Path.Combine(path, SAVED_CLIPS)))
                {
                    events.Add(await LoadEvent(evd));
                }
            }
            if (Directory.Exists(Path.Combine(path, SENTRY_CLIPS)))
            {
                foreach (var evd in Directory.GetDirectories(Path.Combine(path, SENTRY_CLIPS)))
                {
                    events.Add(await LoadEvent(evd));
                }
            }
            events.RemoveAll(e => e is null);
            return events;
        }

        private static async Task<Event> LoadEvent(string path)
        {
            try
            {
                EventMetadata metadata = null;
                if (File.Exists(Path.Combine(path, "event.json")))
                {
                    metadata = JsonConvert.DeserializeObject<EventMetadata>(await File.ReadAllTextAsync(Path.Combine(path, "event.json")));
                }

                var videoFileGroups = from f in new DirectoryInfo(path).GetFiles("*" + EXT)
                                      where f.Length >= FILE_LENGTH_SKIP_THRESHOLD
                                      let prefix = GetFileTimestampPrefix(f.Name)
                                      group f by prefix into timeGroup
                                      select timeGroup;

                if (!videoFileGroups.Any())
                {
                    return null;
                }

                var clipsets = from g in videoFileGroups
                               select new Clipset
                               {
                                   StartTime = DateTime.ParseExact(GetFileTimestampPrefix(g.First().Name), "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                                   DurationSeconds = GetVideoDuration(g.First().FullName).TotalSeconds,
                                   FrontCamera = g.First(f => f.Name.Contains(FRONT_SUFFIX)).FullName,
                                   RearCamera = g.First(f => f.Name.Contains(REAR_SUFFIX)).FullName,
                                   RightRepeaterCamera = g.First(f => f.Name.Contains(RIGHT_RPTR_SUFFIX)).FullName,
                                   LeftRepeaterCamera = g.First(f => f.Name.Contains(LEFT_RPTR_SUFFIX)).FullName
                               };

                return new Event
                {
                    Clips = clipsets.ToList(),
                    Metadata = metadata,
                    Type = path.Contains(RECENT_CLIPS)
                            ? EventType.Recent
                            : path.Contains(SENTRY_CLIPS)
                                ? EventType.Sentry
                                : path.Contains(SAVED_CLIPS)
                                    ? EventType.Saved
                                    : EventType.Unknown
                };
            }
            catch (Exception ex)
            {
                throw new CameraEventLoadException("Unable to load camera event: " + path, ex);
            }
        }

        private static string GetFileTimestampPrefix(string fileName) => fileName.Replace(FRONT_SUFFIX, "").Replace(REAR_SUFFIX, "").Replace(LEFT_RPTR_SUFFIX, "").Replace(RIGHT_RPTR_SUFFIX, "");

        /// <summary>
        /// Gets the video duration of a video file.
        /// </summary>
        /// <remarks>Thanks @MarkHeath - https://markheath.net/post/how-to-get-media-file-duration-in-c</remarks>
        public static TimeSpan GetVideoDuration(string filePath)
        {
            try
            {
                using var shell = ShellObject.FromParsingName(filePath);
                return TimeSpan.FromTicks((long)(ulong)shell.Properties.System.Media.Duration.ValueAsObject);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }
}
