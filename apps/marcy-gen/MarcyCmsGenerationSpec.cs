using CandyKingdom.Marcy;
using CandyKingdom.MarcyCms.Settings;

using TypeGen.Core.SpecGeneration;

namespace CandyKingdom.MarcyGen;

public sealed class MarcyCmsGenerationSpec : GenerationSpec
{
    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        args.GeneratorOptions.PropertyNameConverters.Add(new JsonMemberNameConverter());
        args.GeneratorOptions.TypeNameConverters.Add(new TypeNameConverter());
        args.GeneratorOptions.FileNameConverters.Add(new FileNameConverter());

        AddBarrel("", BarrelScope.Files);

        AddInterface<SettingGroup>()
            .Member(x => nameof(x.Records)).Type($"{nameof(Setting)}<any>[]", "./setting");

        AddInterface<Setting>();
        AddInterface(typeof(Setting<>));

        AddInterface<SettingData>();

        AddInterface<TextSettingData>()
            .Member(nameof(LocalizedTextSettingData.SettingDataType)).Ignore();

        AddInterface<LocalizedTextSettingData>()
            .Member(x => nameof(x.Text)).Type(nameof(LocalizedString), "@candy-kingdom/bonnie")
            .Member(nameof(LocalizedTextSettingData.SettingDataType)).Ignore();

        AddInterface<SvgSettingData>()
            .Member(x => nameof(x.Src)).Type("FileSrcImageMeta", "@candy-kingdom/bonnie")
            .Member(nameof(SvgSettingData.SettingDataType)).Ignore();

        AddInterface<FileSettingData>()
            .Member(x => nameof(x.Src)).Type("FileSrcFileMeta", "@candy-kingdom/bonnie")
            .Member(nameof(FileSettingData.SettingDataType)).Ignore();

        AddInterface<LottieSettingData>()
            .Member(x => nameof(x.Src)).Type("FileSrcFileMeta", "@candy-kingdom/bonnie")
            .Member(nameof(LottieSettingData.SettingDataType)).Ignore();

        AddEnum<TextSettingType>();
    }
}
