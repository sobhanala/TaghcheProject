namespace HelloMicroServices.Infrastructure;

public sealed class BookCacheOptions
{
    public const string SectionName = "BookCache";

    public string RedisConnectionString { get; init; } = "localhost:6379";

    public string RedisKeyPrefix { get; init; } = "books";

    public TimeSpan MemoryDuration { get; init; } = TimeSpan.FromMinutes(5);

    public TimeSpan RedisDuration { get; init; } = TimeSpan.FromHours(1);
}
