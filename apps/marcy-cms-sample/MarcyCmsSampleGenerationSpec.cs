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
    public static InterfaceSpecBuilder<T> IgnoreMember<T, TProp>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, TProp>> propSelector)
    {
        var expression = (MemberExpression)propSelector.Body;

        builder
            .Member(expression.Member.Name)
            .Ignore();

        return builder;
    }
    public static InterfaceSpecBuilder<T> InheritedFromBone<T>(this InterfaceSpecBuilder<T> builder, string type)
        where T : Bone
    {
        builder.IgnoreBase().CustomBase(nameof(Bone), MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);
        builder.Member(x => nameof(x.Type)).Type($"'{type}'");

        builder.Member(x => "BoneType").MemberName("$type").Type($"'{type}'"); // todo: remove. its a deserialization fix

        return builder;
    }

    public static InterfaceSpecBuilder<T> HasLocalizedString<T>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, LocalizedString>> propSelector)
    {
        var expression = (MemberExpression)propSelector.Body;

        builder
            .Member(expression.Member.Name)
            .Type(nameof(LocalizedString), MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);

        return builder;
    }

    public static InterfaceSpecBuilder<T> HasMedia<T>(this InterfaceSpecBuilder<T> builder, Expression<Func<T, PixMedia>> propSelector)
    {
        var expression = (MemberExpression)propSelector.Body;

        builder
            .Member(expression.Member.Name)
            .Type("PixMediaUnion", MarcyCmsSampleGenerationSpec.BonnieModuleImportPath);

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
            .InheritedFromBone(MediaBone.BoneType)
            .HasLocalizedString(x => x.Title)
            .HasLocalizedString(x => x.Link)
            .HasLocalizedString(x => x.Text)
            .HasLocalizedString(x => x.Alt)
            .HasMedia(x => x.Media);


        AddClass<MediaBoneStyle>();

        AddInterface<TextBone>()
            .InheritedFromBone(TextBone.BoneType)
            .HasLocalizedString(x => x.Title)
            .HasLocalizedString(x => x.Text);

        AddClass<TextBoneStyle>();

        AddInterface<PageListBone>()
            .InheritedFromBone(PageListBone.BoneType)
            .HasLocalizedString(x => x.Title)
            .HasBonnieMember(x => x.Data, $"PageBase[]");

        AddClass<PageListBoneStyle>();
    }
}
