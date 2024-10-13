using CatApi.Models.Entities;

namespace CatApi.Services.Interfaces;

public interface IStoreImageService
{
    public Task<byte[]?> GetCatImageFromUrl(string url);

    public Task StoreCatImage(CatEntity cat, string url);
}
