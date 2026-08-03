using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using HelloMicroServices.Domain;

namespace HelloMicroServices.Infrastructure;

public sealed class TaaghcheBookDataSource(
    IHttpClientFactory httpClientFactory) : GetData
{
    public const string HttpClientName = "TaaghcheBooks";

    public GetData? Next { get; set; }

    public async Task<Book?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        var httpClient = httpClientFactory.CreateClient(HttpClientName);
        using var response = await httpClient.GetAsync($"v2/book/{id}");

        Book? book = null;

            response.EnsureSuccessStatusCode();

            var payload =
                await response.Content.ReadFromJsonAsync<TaaghcheBookResponse>();
            var responseBook = payload?.Book;

            book = new Book(responseBook.Id, responseBook.Title);
            

        if (book != null)
        {
            return book;
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
        return Task.CompletedTask;
    }

    private sealed record TaaghcheBookResponse(
        [property: JsonPropertyName("book")] TaaghcheBook? Book);

    private sealed record TaaghcheBook(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("title")] string? Title);
}
