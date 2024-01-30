using System.Linq.Expressions;

using CandyKingdom.Marcy;
using CandyKingdom.Marcy.Pages;
using CandyKingdom.Marcy.Skeleton;
using CandyKingdom.MarcyCms.Sample.Bones;
using CandyKingdom.MarcyCms.Sample.PageTypes;

using TypeGen.Core.SpecGeneration;
using TypeGen.Core.SpecGeneration.Generic;

namespace CandyKingdom.MarcyCms.Sample;

public static class GenExtensions
{
    public static InterfaceSpecBuilder<T> InheritedFromBone<T>(this InterfaceSpecBuilder<T> builder)
        where T: Bone
    {
        builder.IgnoreBase().CustomBase(nameof(Bone), "@candy-kingdom/bonnie");
        return builder;
    }

    public static InterfaceSpecBuilder<T> HasLocalizedString<T, TProp>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, TProp>> propSelector)
    {
        var expression = (MemberExpression)propSelector.Body;

        return builder.Member(expression.Member.Name).Type(nameof(LocalizedString), "@candy-kingdom/bonnie");
    }
}

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
            .HasLocalizedString(x => x.SpecialTitle)
            .Member(x => nameof(x.PageDataType)).Ignore();

        AddInterface<MediaBone>()
            .InheritedFromBone()
            .HasLocalizedString(x => x.Title)
            .HasLocalizedString(x => x.Link)
            .HasLocalizedString(x => x.Text)
            .HasLocalizedString(x => x.Alt)
            .Member(x => nameof(x.Media)).Type(nameof(PixMedia), "@candy-kingdom/bonnie");
    }
}
