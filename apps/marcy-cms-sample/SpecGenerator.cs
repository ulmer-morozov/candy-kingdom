using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

namespace CandyKingdom.MarcyCms.Sample;

public static class SpecGenerator
{
    public static void GenerateTsFiles<T>(string outputDirectory)
       where T : GenerationSpec, new()
    {
        var customMappings = new Dictionary<string, string>();

        var generator = new Generator(
          options: new GeneratorOptions
          {
              BaseOutputDirectory = outputDirectory,
              CustomTypeMappings = customMappings
          }
        );

        generator.Generate(new[] { new T() }); // generates the files
    }

}
