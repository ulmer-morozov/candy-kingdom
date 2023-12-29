namespace CandyKingdom.Marcy.Utilities
{
    public static class FileUtils
    {
        public static DirectoryInfo CreateTempDir()
        {
            var outDir = new DirectoryInfo(
                Path.Combine(GetTempPath(), Guid.NewGuid().ToString().Replace('-', '_'))
            );

            outDir.Create();

            return outDir;
        }

        public static string CreateTempFilePath(string prefix, string extension)
        {
            var uniqueFileName = $"{prefix}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(GetTempPath(), uniqueFileName);

            return filePath;
        }

        private static string GetTempPath()
        {
            return "";
            // return Path.GetTempPath();
        }
    }
}
