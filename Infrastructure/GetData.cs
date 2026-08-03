using HelloMicroServices.Domain;

namespace HelloMicroServices.Infrastructure;

public interface GetData
{
    GetData? Next { get; set; }

    GetData SetNext(GetData next)
    {
        Next = next;
        return next;
    }

    Task<Book?> GetByIdAsync(int id);

    Task SetAsync(Book book)
    {
        return Task.CompletedTask; 
    }
}
