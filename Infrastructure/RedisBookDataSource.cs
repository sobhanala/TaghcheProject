using HelloMicroServices.Domain;
using HelloMicroServices.Infrastructure;
using StackExchange.Redis;
using System.Text.Json;

public class RedisBookDataSource : GetData
{

    public GetData? Next { get; set; }
    private static readonly ConnectionMultiplexer _redis =
        ConnectionMultiplexer.Connect("localhost:6379");

    private static readonly IDatabase _database = _redis.GetDatabase();

    public RedisBookDataSource()
    {
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var value = await _database.StringGetAsync(id.ToString());

        if (value.HasValue)
        {
            return JsonSerializer.Deserialize<Book>(value!.ToString());
        }

        if (Next == null)
            return null;

        var book = await Next.GetByIdAsync(id);

        if (book != null)
            await SetAsync(book);

        return book;
    }

    public async Task SetAsync(Book book)
    {
        var json = JsonSerializer.Serialize(book);

        await _database.StringSetAsync(
            book.Id.ToString(),
            json);
    }
}