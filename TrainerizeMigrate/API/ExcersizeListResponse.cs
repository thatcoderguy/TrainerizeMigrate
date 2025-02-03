using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class ExcersizeListResponse
    {
        public List<Exercise> exercises { get; set; }
    }

    public class AudioUrl
    {
        public string male { get; set; }
        public string female { get; set; }
    }

    public class Default
    {
        public string videoToken { get; set; }
        public string loopVideoToken { get; set; }
        public VideoUrls videoUrls { get; set; }
        public LoopVideoUrls loopVideoUrls { get; set; }
        public ThumbnailUrls thumbnailUrls { get; set; }
    }

    public class Exercise
    {
        public int id { get; set; }
        public string name { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string recordType { get; set; }
        public Media media { get; set; }
        public string videoType { get; set; }
        public string videoUrl { get; set; }
        public VideoMobileUrl videoMobileUrl { get; set; }
        public string videoStatus { get; set; }
        public int numPhotos { get; set; }
        public List<Tag> tags { get; set; }
        public string lastPerformed { get; set; }
        public string version { get; set; }
    }

    public class Female
    {
        public string videoToken { get; set; }
        public string loopVideoToken { get; set; }
        public VideoUrls videoUrls { get; set; }
        public LoopVideoUrls loopVideoUrls { get; set; }
        public ThumbnailUrls thumbnailUrls { get; set; }
    }

    public class LoopVideoUrl
    {
        public string sd { get; set; }
        public object hd { get; set; }
        public string fhd { get; set; }
        public string hls { get; set; }
        public object hlssd { get; set; }
        public object hlshd { get; set; }
    }

    public class LoopVideoUrls
    {
        public string sd { get; set; }
        public object hd { get; set; }
        public string fhd { get; set; }
        public string hls { get; set; }
        public object hlssd { get; set; }
        public object hlshd { get; set; }
    }

    public class Media
    {
        public bool isOverride { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        public string loopVideoToken { get; set; }
        public VideoUrl videoUrl { get; set; }
        public LoopVideoUrl loopVideoUrl { get; set; }
        public ThumbnailUrl thumbnailUrl { get; set; }
        public AudioUrl audioUrl { get; set; }
        public Default @default { get; set; }
        public Female female { get; set; }
    }

    public class Tag
    {
        public string type { get; set; }
        public string name { get; set; }
    }

    public class ThumbnailUrl
    {
        public string hd { get; set; }
        public string sd { get; set; }
    }

    public class ThumbnailUrls
    {
        public string hd { get; set; }
        public string sd { get; set; }
    }

    public class VideoMobileUrl
    {
        public string mobile { get; set; }
        public object hd { get; set; }
        public string fhd { get; set; }
        public string hls { get; set; }
        public object hlssd { get; set; }
        public object hlshd { get; set; }
    }

    public class VideoUrl
    {
        public string sd { get; set; }
        public object hd { get; set; }
        public string fhd { get; set; }
        public string hls { get; set; }
        public object hlssd { get; set; }
        public object hlshd { get; set; }
    }

    public class VideoUrls
    {
        public string sd { get; set; }
        public object hd { get; set; }
        public string fhd { get; set; }
        public string hls { get; set; }
        public object hlssd { get; set; }
        public object hlshd { get; set; }
    }


}
