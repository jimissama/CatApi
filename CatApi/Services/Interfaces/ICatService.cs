using CatApi.Models.Response;

namespace CatApi.Services.Interfaces;

public interface ICatService
{
    public Task<int> FetchCatsAsync();

    public Task<List<CatResponse>> GetCatsAsync(string? tag, int page, int pageSize);

    public Task<CatResponse?> GetCatByIdAsync(int id);
}
