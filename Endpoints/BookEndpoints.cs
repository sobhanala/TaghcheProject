using HelloMicroServices.Services;

namespace HelloMicroServices.Endpoints;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/books")
            .WithTags("Books");

        group.MapGet("/{id:int:min(1)}", GetByIdAsync);

        return endpoints;
    }

    private static async Task<IResult> GetByIdAsync(
        int id,
        IBookService bookService)
    {
        var book = await bookService.GetByIdAsync(id);

        return book is null
            ? Results.NotFound()
            : Results.Ok(book);
    }
}
