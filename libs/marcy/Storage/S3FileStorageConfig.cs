namespace CandyKingdom.Marcy.Storage;

public sealed record S3FileStorageConfig : ConfigurationBase
{
    public required string Host { get; init; }
    public required string BucketName { get; init; }
    public required string ObjectStorageKeyId { get; init; }
    public required string ObjectStorageKey { get; init; }

    public override void Verify()
    {
        ThrowIfNullOrWhiteSpace(Host, $"{nameof(Host)} can not be empty");
        ThrowIfNullOrWhiteSpace(BucketName, $"{nameof(BucketName)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKeyId, $"{nameof(ObjectStorageKeyId)} can not be empty");
        ThrowIfNullOrWhiteSpace(ObjectStorageKey, $"{nameof(ObjectStorageKey)} can not be empty");
    }

    public static S3FileStorageConfig CreateFromEnv()
    {
        var config = new S3FileStorageConfig
        {
            Host = GetEnvVarOrThrow("S3_HOST"),
            BucketName = GetEnvVarOrThrow("S3_BUCKET_NAME"),
            ObjectStorageKey = GetEnvVarOrThrow("S3_OBJECT_STORAGE_KEY"),
            ObjectStorageKeyId = GetEnvVarOrThrow("S3_OBJECT_STORAGE_KEY_ID")
        };

        config.Verify();

        return config;
    }
}
