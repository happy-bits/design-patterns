using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DesignPatterns.Proxy.Youtubes
{
    [TestClass]
    public class Tests
    {
        private IEnumerable<IClient> AllClients() => new IClient[] {
            new Before.Client(),
            new After.Client(),
        };

        [TestMethod]
        public void Ex1()
        {
            foreach (var client in AllClients())
            {
                client.DoStuff();
            }
        }
    }

    class Video
    {
        public string Name { get; set; }
        public int Length { get; set; }
    }
    class DownloadedVideo : Video
    {
        public byte[] Content { get; set; }
    }

    interface IThirdPartyYoutube
    {
        IEnumerable<Video> ListVideos();
        Video GetVideoInfo(int id);
        DownloadedVideo DownloadVideo(int id);
    }

    class ThirdPartyYoutube : IThirdPartyYoutube
    {
        private Queue<string> _events = new Queue<string>();

        public IEnumerable<string> Events => _events;

        public DownloadedVideo DownloadVideo(int id)
        {
            _events.Enqueue($"Download video {id}");
            return new DownloadedVideo
            {
                Name = "A",
                Content = new byte[] { 1, 2, 3, 4 },
                Length = 1234
            };
        }

        public Video GetVideoInfo(int id)
        {
            _events.Enqueue($"Get video {id}");
            return new Video
            {
                Name = "A",
                Length = 1234
            };
        }

        public IEnumerable<Video> ListVideos()
        {
            _events.Enqueue($"List all videos");
            return new Video[] {
                    new Video{Name="A", Length=1234},
                    new Video{Name="B", Length=2345},
                    new Video{Name="C", Length=5678},

                };

        }
    }
}
