using CandyKingdom.Marcy;
using CandyKingdom.Marcy.ImageTools;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace CandyKingdom.MarcyGen;

public sealed class MarcyGenerationSpec : GenerationSpec
{
    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        args.GeneratorOptions.PropertyNameConverters.Add(new JsonMemberNameConverter());
        args.GeneratorOptions.TypeNameConverters.Add(new TypeNameConverter());
        args.GeneratorOptions.FileNameConverters.Add(new FileNameConverter());

        AddBarrel("", BarrelScope.Files);

        AddInterface(typeof(LocalizedObject<>))
          .Member("Localizations").MemberName("[locale: string]").Type("T")
          .Member("EnCode").Ignore();

        AddInterface<LocalizedString>()
          .Member("Empty")
          .Ignore()
          .Member(x => nameof(x.En))
          .Ignore();

        AddInterface<Bone>();

        AddInterface<PixMedia>().Member(x => nameof(x.Type)).Type("'image' | 'video'");

        AddInterface<Image>()
            .Member(x => nameof(x.Type)).Type($"'{Image.MediaType}'")
            .Member(x => nameof(x.MediaType)).MemberName("$type").Type($"'{Image.MediaType}'"); // todo: remove

        AddInterface<Video>()
            .Member(x => nameof(x.Type)).Type($"'{Video.MediaType}'")
            .Member(x => nameof(x.MediaType)).MemberName("$type").Type($"'{Video.MediaType}'"); // todo: remove

        AddInterface(typeof(ImageSource));
        AddInterface(typeof(VideoSource));
        AddInterface(typeof(MediaSourceBase));
        AddInterface(typeof(MediaSource<>));

        AddInterface(typeof(FileSrc<>));
        AddInterface(typeof(FileSrcBase));

        AddInterface<FileMeta>().Member(x => nameof(x.Empty)).Ignore();
        AddInterface<PixMeta>();
        AddInterface<SvgMeta>();
        AddInterface<ImageMeta>().Member(x => nameof(x.Empty)).Ignore();
        AddInterface<VideoMeta>().Member(x => nameof(x.Duration)).Type(TsType.Number);

        AddInterface<SizesItem>();
        AddEnum<SizesWidthUnit>().StringInitializers();

        AddInterface(typeof(IHaveDataRouteWithData<>));

        AddEnum<PublishStatus>();

        AddInterface(typeof(IHaveSkeleton));

        AddInterface(typeof(View));
        AddInterface(typeof(Page));
        AddInterface(typeof(Page<>));

        AddInterface<PageData>()
            .Member(nameof(PageData.Empty)).Ignore();

        AddInterface<OpenGraphData>()
            .Member(x => nameof(x.Empty)).Ignore();

        AddInterface<FileFormat>()
            .Member(nameof(FileFormat.Empty)).Ignore();
    }
}
