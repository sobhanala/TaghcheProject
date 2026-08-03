using HelloMicroServices.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HelloMicroServices.Infrastructure;

public sealed class MemoryBookDataSource(
    IMemoryCache memoryCache,
    IOptions<BookCacheOptions> cacheOptions) : GetData
{
    private readonly BookCacheOptions _cacheOptions = cacheOptions.Value;

    public GetData? Next { get; set; }

    public async Task<Book?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        if (memoryCache.TryGetValue<Book>((id), out var book))
        {
            return book;
        }

        if (Next is null)
        {
            return null;
        }

        book = await Next.GetByIdAsync(id);

        if (book is not null)
        {
            await SetAsync(book);
        }

        return book;
    }

    public Task SetAsync(Book book)
    {
        memoryCache.Set(
            (book.Id),
            book,
            _cacheOptions.MemoryDuration);

        return Task.CompletedTask;
    }

}
