using HelloMicroServices.Endpoints;
using HelloMicroServices.Infrastructure;
using HelloMicroServices.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.Configure<BookCacheOptions>(
    builder.Configuration.GetSection(BookCacheOptions.SectionName));
builder.Services.AddHttpClient(
    TaaghcheBookDataSource.HttpClientName,
    client =>
    {
        var baseUrl = builder.Configuration["BookApi:BaseUrl"]
            ?? "https://get.taaghche.com/";
        client.BaseAddress = new Uri(baseUrl);
    });

builder.Services.AddSingleton<MemoryBookDataSource>();
builder.Services.AddSingleton<RedisBookDataSource>();
builder.Services.AddSingleton<TaaghcheBookDataSource>();
builder.Services.AddSingleton<GetData>(services =>
{
    GetData memorySource =
        services.GetRequiredService<MemoryBookDataSource>();
    GetData redisSource =
        services.GetRequiredService<RedisBookDataSource>();
    GetData apiSource =
        services.GetRequiredService<TaaghcheBookDataSource>();

    memorySource
        .SetNext(redisSource)
        .SetNext(apiSource);

    return memorySource;
});
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapBookEndpoints();

app.Run();
