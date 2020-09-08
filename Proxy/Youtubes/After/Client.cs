
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Proxy.Youtubes.After
{
    class Client : IClient
    {
        public void DoStuff()
        {

            var c = new CachedYoutube(new ThirdPartyYoutube());
            c.GetVideoInfo(100);
            c.GetVideoInfo(100);
            c.DownloadVideo(100);
            c.DownloadVideo(100);
            c.ListVideos();

            CollectionAssert.AreEqual(new[] {
                "Get video 100",
                "Get video 100 from cache",
                "Download video 100",
                "Download video 100 from cache",
                "List all videos"
            }, c.Events.ToArray());
        }
    }

    class CachedYoutube : IThirdPartyYoutube
    {
        private Queue<string> _events = new Queue<string>();

        private readonly ThirdPartyYoutube _thirdPartyYoutube;

        private Dictionary<int, Video> _cached = new Dictionary<int, Video>();

        public CachedYoutube(ThirdPartyYoutube thirdPartyYoutube)
        {
            _thirdPartyYoutube = thirdPartyYoutube;
        }

        public IEnumerable<string> Events => _events;

        public DownloadedVideo DownloadVideo(int id)
        {
            if (_cached.ContainsKey(id) && _cached[id] is DownloadedVideo)
            {
                _events.Enqueue($"Download video {id} from cache");

                return (DownloadedVideo)_cached[id];
            }

            var downloaded = _thirdPartyYoutube.DownloadVideo(id);

            if (_cached.ContainsKey(id))
                _cached.Remove(id);

            _cached.Add(id, downloaded);

            _events.Enqueue($"Download video {id}");
            return downloaded;
        }

        public Video GetVideoInfo(int id)
        {
            if (_cached.ContainsKey(id))
            {
                _events.Enqueue($"Get video {id} from cache");
                return _cached[id];
            }
            var video = _thirdPartyYoutube.GetVideoInfo(id);
            _cached.Add(id, video);
            _events.Enqueue($"Get video {id}");
            return video;
        }

        // Inget intressant här, inget cache'as
        public IEnumerable<Video> ListVideos()
        {
            _events.Enqueue($"List all videos");
            return _thirdPartyYoutube.ListVideos();
        }
    }
}
