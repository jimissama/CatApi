using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Services.Interfaces;

public interface ICatTagService
{
    public Task StoreTags(IEnumerable<CatTagEntity> cattags, CatDbContext context);
    public Task StoreTags(IEnumerable<CatTagEntity> cattags);
}
