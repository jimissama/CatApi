using System.Resources;
using AutoMapper;
using CatApi.Extensions;
using CatApi.Models.Entities;
using CatApi.Models.Response;
using CatApi.Models.SourceApi;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;
using CatApi.Services.Interfaces;
using Newtonsoft.Json;

namespace CatApi.Services;

public class CatService : ICatService
{
    private readonly ICatRepo _catRepo;
    private readonly ITagService _tagService;
    private readonly ICatTagService _catTagService;
    private readonly IHttpRequestService _httpRequestService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    private readonly ResourceManager _resourceManager;

    public CatService(IServiceProvider serviceProvider, IConfiguration configuration, ResourceManager resourceManager)
    {
        _catRepo = serviceProvider.GetRequiredService<ICatRepo>();
        _tagService = serviceProvider.GetRequiredService<ITagService>();
        _catTagService = serviceProvider.GetRequiredService<ICatTagService>();
        _httpRequestService = serviceProvider.GetRequiredService<IHttpRequestService>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();

        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _resourceManager = resourceManager;
    }

    public async Task<int> FetchCats()
    {
        int fetchCount = 0;

        var catApiKey = _configuration.GetSection("CatApiSettings:CatApiKey").Value;

        if (string.IsNullOrEmpty(catApiKey))
            throw new Exception(_resourceManager.GetString("CatApiKeyNotFound"));

        var httpStringResponse = await _httpRequestService.HttpGet(new Dictionary<string, string> { { "x-api-key", catApiKey } },
                                                                   _configuration.GetSection("CatApiSettings:CatApiUrl").Value ?? string.Empty);

        var cats = GetCatSourceFromResponse(httpStringResponse);

        if (cats is null)
            throw new Exception(_resourceManager.GetString("CatNotFetched"));

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();


            var catTagData = await GetCatTagData(cats, context);


            await _catRepo.StoreCats(catTagData.NewCats, context);

            await _tagService.StoreTags(catTagData.NewTags, context);


            await context.SaveChangesAsync();


            var allCats = await _catRepo.GetExistingCats(catTagData.NewCats.ToList(), context);

            var allTags = await _tagService.GetExistingTags(catTagData.NewCatsAllTags, context);

            var catTagMappings = catTagData.NewCatTagsDict.SelectMany(ct => ct.Value.Select(val =>
            {
                var cat = allCats.First(ac => ac.CatId.Equals(ct.Key, StringComparison.InvariantCultureIgnoreCase));
                var tag = allTags.First(at => at.Name.Equals(val, StringComparison.InvariantCultureIgnoreCase));
                return new CatTagEntity { CatId = cat.Id, Cat = cat, Tag = tag, TagId = tag.Id };

            }));

            await _catTagService.StoreTags(catTagMappings, context);

            await context.SaveChangesAsync();

            fetchCount = catTagData.NewCats.Count();
        }

        return fetchCount;
    }

    private async Task<(IEnumerable<CatEntity> NewCats,
                        IEnumerable<TagEntity> NewCatsAllTags,
                        IEnumerable<TagEntity> NewTags,
                        Dictionary<string,
                        IEnumerable<string>> NewCatTagsDict)> GetCatTagData(IEnumerable<CatSourceResponse> cats,
                                                                            CatDbContext context)
    {
        var existingCatIds = await _catRepo.GetExistingCatIds(cats.Select(c => c.Id).ToList(), context);

        var newCats = _mapper.Map<IEnumerable<CatEntity>>(cats.Where(c => !existingCatIds.Contains(c.Id, new StringEqualityExtension()))); //Map from CatApiDto to CatEntity only for not existing cat records

        if (newCats is null)
            throw new Exception(_resourceManager.GetString("CatNotStored"));

        // new Cat Tags Dictionary
        var newCatTagsDict = cats.GetCatTagsDictionary(existingCatIds);

        // all tags of the new Cats
        var tags = newCatTagsDict.GetTagEntities();

        var existingTagNames = await _tagService.GetExistingTagNames(tags, context);

        // new tags
        var newTags = tags.Where(tag => !existingTagNames.Contains(tag.Name, new StringEqualityExtension())).ToList();

        return (newCats, tags, newTags, newCatTagsDict);
    }

    private IEnumerable<CatSourceResponse>? GetCatSourceFromResponse(string httpStringResponse)
    {
        try
        {
            IEnumerable<CatSourceResponse>? cats = JsonConvert.DeserializeObject<IEnumerable<CatSourceResponse>>(httpStringResponse);
            return cats;
        }
        catch (Exception ex)
        {
            throw new Exception(_resourceManager.GetString("SourceNotValid"), ex);
        }
    }

    public async Task<List<CatResponse>> GetCats(string? tag, int page, int pageSize)
    {
        var cats = await _catRepo.GetCats(tag, page, pageSize);

        if (cats is null || cats.Count == 0)
            return new List<CatResponse>();

        return _mapper.Map<List<CatResponse>>(cats);
    }

    public async Task<CatResponse?> GetCatById(int id)
    {
        var cat = await _catRepo.GetCatById(id);

        if (cat is null)
            return null;

        return _mapper.Map<CatResponse>(cat);
    }
}
