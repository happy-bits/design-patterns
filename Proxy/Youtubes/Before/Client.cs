
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DesignPatterns.Proxy.Youtubes.Before
{
    class Client : IClient
    {
        public void DoStuff()
        {

            var c = new ThirdPartyYoutube();
            c.GetVideoInfo(100);
            c.GetVideoInfo(100);
            c.DownloadVideo(100);
            c.DownloadVideo(100);
            c.ListVideos();

            CollectionAssert.AreEqual(new[] {
                "Get video 100",
                "Get video 100",      // nackdel: inget cache'as
                "Download video 100",
                "Download video 100", // nackdel: inget cache'as
                "List all videos"
            }, c.Events.ToArray());

            // Fördel: ingen extra kod
        }


    }
}
