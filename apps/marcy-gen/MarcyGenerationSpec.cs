using System.Collections.Immutable;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;

using TypeGen.Core.Converters;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace CandyKingdom.MarcyGen;


public class TypeNameConverter : ITypeNameConverter
{
    // These needed for successfull generation of generic classes that have same name as base class
    public static readonly ImmutableList<Type> SpecialTypes = [typeof(Page)];

    public string Convert(string name, Type type)
    {
        if (SpecialTypes.Contains(type))
        {
            return $"{name}Base";
        }

        return name;
    }
}

public class FileNameConverter : ITypeNameConverter
{
    public string Convert(string name, Type type)
    {
        if (TypeNameConverter.SpecialTypes.Contains(type))
        {
            return $"{name}-base";
        }

        return name;
    }
}

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
            .Member(x => nameof(x.Type)).Type("'image'")
            .Member(x => nameof(x.MediaType)).Ignore();

        AddInterface<Video>()
            .Member(x => nameof(x.Type)).Type("'video'")
            .Member(x => nameof(x.MediaType)).Ignore();

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

        AddEnum<PublishStatus>();

        AddInterface(typeof(IHaveSkeleton));

        AddInterface(typeof(View));
        AddInterface(typeof(Page));
        AddInterface(typeof(Page<>));

        AddInterface<PageData>()
            .Member(nameof(PageData.Empty)).Ignore();

        AddInterface<OpenGraphData>();

    }
}
