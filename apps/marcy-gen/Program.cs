using CandyKingdom.MarcyGen;

using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

var generatedContractsFolder = Path.Combine(
          "..",
          "..",
          "libs",
          "bonnie",
          "src",
          "lib",
          "generated"
        );

GenerateTsFiles<MarcyGenerationSpec>(generatedContractsFolder);

return 0;

static void GenerateTsFiles<T>(string outputDirectory)
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
