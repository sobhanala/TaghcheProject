using HelloMicroServices.Domain;
using HelloMicroServices.Infrastructure;

namespace HelloMicroServices.Services;

public sealed class BookService(GetData bookDataSource) : IBookService
{
    public Task<Book?> GetByIdAsync(int id)
    {
        return bookDataSource.GetByIdAsync(id);
    }
}
