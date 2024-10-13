using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Services.Interfaces;

public interface ICatTagService
{
    public Task StoreTagsAsync(IEnumerable<CatTagEntity> cattags, CatDbContext context);
    public Task StoreTagsAsync(IEnumerable<CatTagEntity> cattags);
}
