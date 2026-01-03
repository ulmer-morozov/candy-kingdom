using CandyKingdom.MarcyGen;

using TypeGen.Core.Generator;
using TypeGen.Core.SpecGeneration;

var bonnieDir = Path.Combine("libs", "bonnie", "src", "lib", "generated");
var bonnieCmsDir = Path.Combine("libs", "bonnie-cms", "src", "lib", "generated");

Console.WriteLine("Generating TypeScript files...");

GenerateTsFiles<MarcyGenerationSpec>(bonnieDir);
GenerateTsFiles<MarcyCmsGenerationSpec>(bonnieCmsDir);

return 0;

static void GenerateTsFiles<T>(string outputDirectory)
  where T : GenerationSpec, new()
{
  Console.WriteLine("Output directory: " + Path.GetFullPath(outputDirectory));

    var customMappings = new Dictionary<string, string>();

    var generator = new Generator(
      options: new GeneratorOptions
      {
          BaseOutputDirectory = outputDirectory,
          CustomTypeMappings = customMappings
      }
    );

    generator.Generate([new T()]); // generates the files
}
