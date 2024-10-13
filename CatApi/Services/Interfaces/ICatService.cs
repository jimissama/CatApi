using CatApi.Models.Response;

namespace CatApi.Services.Interfaces;

public interface ICatService
{
    public Task<int> FetchCats();

    public Task<List<CatResponse>> GetCats(string? tag, int page, int pageSize);

    public Task<CatResponse?> GetCatById(int id);
}
