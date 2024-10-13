using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Repositories.Interfaces;

public interface ICatTagRepo
{
    public Task StoreTagsAsync(IEnumerable<CatTagEntity> cattags, CatDbContext context);
    public Task StoreTagsAsync(IEnumerable<CatTagEntity> cattags);
}
