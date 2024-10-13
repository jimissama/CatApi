using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Repositories.Interfaces;

public interface ICatRepo
{
    public Task StoreCatsAsync(IEnumerable<CatEntity> catEntities);

    public Task StoreCatsAsync(IEnumerable<CatEntity> catEntities, CatDbContext context);

    public Task<IEnumerable<CatEntity>> GetExistingCatsAsync(IEnumerable<CatEntity> cats);

    public Task<IEnumerable<CatEntity>> GetExistingCatsAsync(IEnumerable<CatEntity> cats, CatDbContext context);  

    public Task<HashSet<string>> GetExistingCatIdsAsync(IEnumerable<CatEntity> cats);

    public Task<HashSet<string>> GetExistingCatIdsAsync(IEnumerable<CatEntity> cats, CatDbContext context);

    public Task<HashSet<string>> GetExistingCatIdsAsync(IEnumerable<string> catIds);

    public Task<HashSet<string>> GetExistingCatIdsAsync(IEnumerable<string> catIds, CatDbContext context);      

    public Task<List<CatEntity>> GetCatsAsync(string? tag, int page, int pageSize);

    public Task<CatEntity?> GetCatByIdAsync(int id);
}
