using System.Globalization;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.Bones;
using CandyKingdom.MarcyCms.Sample.PageTypes;

using static CandyKingdom.Marcy.LocalizedStringHelpers;
using static CandyKingdom.Marcy.Shugar;

namespace CandyKingdom.MarcyCms.Sample.Content;

public sealed class ProjectA : PageFactory<ProjectPageData>
{
    public override string Route { get; } = "a";
    public override LocalizedString Title { get; } = En("The Project \"A\"");
    public override OpenGraphData OpenGraph { get; } = new OpenGraphData
    {
        Title = En("OpenGraph Title of The Project \"A\""),
        Description = En("OpenGraph Description of The Project \"A\""),
        Image = En("")
    };
    public override ProjectPageData Data { get; } = new ProjectPageData
    {
        SpecialTitle = En("Project A Only Special Title!")
    };

    public ProjectA()
    {
        AddBones(
            new TextBone()
            {
                Title = En("The Project \"A\"")
            },
            new MediaBone
            {
                Style = MediaBoneStyle.MobileDisplay,
                Media = Video
                (
                    VideoSource
                    (
                        new FileSrc<VideoMeta>
                        {
                            Meta = new VideoMeta
                            {
                                ByteCount = 185765954,
                                Width = 1280,
                                Height = 534,
                                Duration = TimeSpan.Parse("00:12:14.26", CultureInfo.InvariantCulture),
                                FullFormat = "h264 (High) (avc1 / 0x31637661), yuv420p(progressive)",
                                HasAudio = true,
                                FrameRate = 24
                            },
                            MimeType = "video/mp4",
                            Url = "https://storage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4"
                        }
                    )
                )
,
                Text = En(
                    "How do you like sample video?"
                )
            },
            new MediaBone
            {
                Media = Image
                (
                    ImageSource
                    (
                        new FileSrc<ImageMeta>
                        {
                            MimeType = "image/jpeg",
                            Meta = new ImageMeta
                            {
                                Width = 480,
                                Height = 360,
                                ByteCount = 21213
                            },
                            Url = "https://storage.googleapis.com/gtv-videos-bucket/sample/images/TearsOfSteel.jpg"
                        }
                    )
                ),
                Text = En("How do you like sample image?")
            },
            new TextBone()
            {
                Text = En(
                    "Sample text about A."
                )
            }
        );
    }
}
