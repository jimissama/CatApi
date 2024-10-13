using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Repositories.Interfaces;

public interface ICatRepo
{
    public Task StoreCats(IEnumerable<CatEntity> catEntities);

    public Task StoreCats(IEnumerable<CatEntity> catEntities, CatDbContext context);

    public Task<IEnumerable<CatEntity>> GetExistingCats(IEnumerable<CatEntity> cats);

    public Task<IEnumerable<CatEntity>> GetExistingCats(IEnumerable<CatEntity> cats, CatDbContext context);  

    public Task<HashSet<string>> GetExistingCatIds(IEnumerable<CatEntity> cats);

    public Task<HashSet<string>> GetExistingCatIds(IEnumerable<CatEntity> cats, CatDbContext context);

    public Task<HashSet<string>> GetExistingCatIds(IEnumerable<string> catIds);

    public Task<HashSet<string>> GetExistingCatIds(IEnumerable<string> catIds, CatDbContext context);      

    public Task<List<CatEntity>> GetCats(string? tag, int page, int pageSize);

    public Task<CatEntity?> GetCatById(int id);
}
