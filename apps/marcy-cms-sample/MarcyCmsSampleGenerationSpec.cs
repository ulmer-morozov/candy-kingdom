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
        where T : Bone
    {
        builder.IgnoreBase().CustomBase(nameof(Bone), MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);
        return builder;
    }

    public static InterfaceSpecBuilder<T> HasLocalizedString<T, TProp>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, TProp>> propSelector)
    {
        var expression = (MemberExpression)propSelector.Body;

        builder
            .Member(expression.Member.Name)
            .Type(nameof(LocalizedString), MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);

        return builder;
    }

    public static InterfaceSpecBuilder<T> HasBonnieMember<T, TProp>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, TProp>> propSelector, string typeName)
    {
        var expression = (MemberExpression)propSelector.Body;

        builder
            .Member(expression.Member.Name)
            .Type(typeName, MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);

        return builder;
    }
}

public sealed class MarcyCmsSampleGenerationSpec : GenerationSpec
{
    public const string BonnieModuleImportPath = "@candy-kingdom/bonnie";

    public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
    {
        // args.GeneratorOptions.PropertyNameConverters.Add(new JsonMemberNameConverter());
        // args.GeneratorOptions.TypeNameConverters.Add(new TypeNameConverter());
        // args.GeneratorOptions.FileNameConverters.Add(new FileNameConverter());

        AddBarrel("", BarrelScope.Files);

        AddInterface<ProjectPageData>()
            .IgnoreBase().CustomBase(nameof(PageData), BonnieModuleImportPath)
            .HasLocalizedString(x => x.SpecialTitle)
            .Member(x => nameof(x.PageDataType)).Ignore();

        AddInterface<MediaBone>()
            .InheritedFromBone()
            .HasLocalizedString(x => x.Title)
            .HasLocalizedString(x => x.Link)
            .HasLocalizedString(x => x.Text)
            .HasLocalizedString(x => x.Alt)
            .Member(x => nameof(x.Media)).Type(nameof(PixMedia), BonnieModuleImportPath);

        AddClass<TextBoneStyle>();

        AddInterface<TextBone>()
            .InheritedFromBone()
            .HasLocalizedString(x => x.Title)
            .HasLocalizedString(x => x.Text);

        AddInterface<PageListBone>()
            .InheritedFromBone()
            .HasLocalizedString(x => x.Title)
            .HasBonnieMember(x => x.Data, $"PageBase[]");

        AddClass<PageListBoneStyle>();
    }
}
