using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.MarcyCms.Sample.PageTypes;

using TypeGen.Core.SpecGeneration;

namespace CandyKingdom.MarcyCms.Sample;

public sealed class MarcyCmsSampleGenerationSpec : GenerationSpec
{
    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        // args.GeneratorOptions.PropertyNameConverters.Add(new JsonMemberNameConverter());
        // args.GeneratorOptions.TypeNameConverters.Add(new TypeNameConverter());
        // args.GeneratorOptions.FileNameConverters.Add(new FileNameConverter());

        AddBarrel("", BarrelScope.Files);

        AddInterface<ProjectPageData>()
            .IgnoreBase().CustomBase(nameof(PageData), "@candy-kingdom/bonnie")
            .Member(x => nameof(x.SpecialTitle)).Type(nameof(LocalizedString), "@candy-kingdom/bonnie")
            .Member(x => nameof(x.PageDataType)).Ignore();
    }
}
