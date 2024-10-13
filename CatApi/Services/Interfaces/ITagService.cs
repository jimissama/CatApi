using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Services.Interfaces;

public interface ITagService
{
    public Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags);

    public Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags);

    public Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task StoreTags(IEnumerable<TagEntity> tags);

    public Task StoreTags(IEnumerable<TagEntity> tags, CatDbContext context);


}
