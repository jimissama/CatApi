using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Repositories.Interfaces;

public interface ITagRepo
{
    public Task<IEnumerable<TagEntity>> GetExistingTagsAsync(IEnumerable<TagEntity> tags);

    public Task<IEnumerable<TagEntity>> GetExistingTagsAsync(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task<HashSet<string>> GetExistingTagNamesAsync(IEnumerable<TagEntity> tags);

    public Task<HashSet<string>> GetExistingTagNamesAsync(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task StoreTagsAsync(IEnumerable<TagEntity> tags);

    public Task StoreTagsAsync(IEnumerable<TagEntity> tags, CatDbContext context);


}
