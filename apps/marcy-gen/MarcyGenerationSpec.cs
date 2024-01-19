using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Skeleton;

using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace CandyKingdom.MarcyGen;

public sealed class MarcyGenerationSpec : GenerationSpec
{
    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        args.GeneratorOptions.PropertyNameConverters.Add(new JsonMemberNameConverter());

        AddBarrel("", BarrelScope.Files);

        AddInterface(typeof(LocalizedObject<>))
          .Member("Localizations")
          .MemberName("[locale: string]")
          .Type("T");

        AddInterface<LocalizedString>()
          .Member("Empty")
          .Ignore()
          .Member(x => nameof(x.En))
          .Ignore();

        AddInterface<Bone>();

        AddInterface<PixMedia>().Member(x => nameof(x.Type)).Type("'image' | 'video'");
        AddInterface<Image>().Member(x => nameof(x.Type)).Type("'image'");
        AddInterface<Video>().Member(x => nameof(x.Type)).Type("'video'");

        AddInterface(typeof(ImageSource));
        AddInterface(typeof(VideoSource));
        AddInterface(typeof(MediaSourceBase));
        AddInterface(typeof(MediaSource<>));

        AddInterface(typeof(FileSrc<>));
        AddInterface(typeof(FileSrcBase));

        AddInterface<FileMeta>();
        AddInterface<PixMeta>();
        AddInterface<ImageMeta>();
        AddInterface<VideoMeta>().Member(x => nameof(x.Duration)).Type(TsType.Number);

        AddInterface<SizesItem>();
        AddEnum<SizesWidthUnit>().StringInitializers();

        AddInterface(typeof(IHaveDataRouteWithData<>));
    }
}
