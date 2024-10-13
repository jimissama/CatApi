using CatApi.Models.Entities;

using CatApi.Services.Interfaces;

namespace CatApi.Services;

[Obsolete]
public class StoreImageService : IStoreImageService
{
    private readonly HttpClient _httpClient;

    private readonly ILogger _logger;

    public StoreImageService(IServiceProvider serviceProvider, ILogger<StoreImageService> logger)
    {
        _httpClient = serviceProvider.GetRequiredService<HttpClient>();
        _logger = logger;

    }


    public async Task<byte[]?> GetCatImageFromUrlAsync(string url)
    {
        try
        {
            return await _httpClient.GetByteArrayAsync(url);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
            return null;
        }
    }

    public async Task StoreCatImageAsync(CatEntity cat, string url)
    {
        //cat.Image = await GetCatImageFromUrl(url);
    }
}
