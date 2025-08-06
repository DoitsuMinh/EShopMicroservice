using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Ordering.API.Youtube;

[Route("api/youtube")]
[ApiController]
public class YoutubeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetYoutubeVid()
    {
        var youtube = new YoutubeClient();
        var videoUrl = "https://music.youtube.com/watch?v=NiqzAybAlqM&si=mdsNXCF2hHGJGVnR";
        // Use the correct method to parse the video ID
        var videoId = VideoId.Parse(videoUrl);

        // Get stream info
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);

        // Choose the best muxed stream (video + audio)
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        if (streamInfo != null)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "video.mp4");
            await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);
        }
        return Ok();
    }
}
