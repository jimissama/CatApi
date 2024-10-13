using CatApi.Models.Entities;

namespace CatApi.Services.Interfaces;

public interface IStoreImageService
{
    public Task<byte[]?> GetCatImageFromUrlAsync(string url);

    public Task StoreCatImageAsync(CatEntity cat, string url);
}
