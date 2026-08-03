using HelloMicroServices.Domain;

namespace HelloMicroServices.Services;

public interface IBookService
{
    Task<Book?> GetByIdAsync(int id);
}
