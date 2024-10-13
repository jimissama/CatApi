using CatApi.Helpers;
using CatApi.Models.Entities;
using CatApi.Models.SourceApi;

namespace CatApi.Extensions;

public static class CatSourceResponseExtensions
{
    public static Dictionary<string, IEnumerable<string>> GetCatTagsDictionary(this IEnumerable<CatSourceResponse> catSourceResponses, HashSet<string> existingCatIds)
    {
        if(existingCatIds is null)
            return new Dictionary<string, IEnumerable<string>>();
        else
            return catSourceResponses.Where(c => !existingCatIds.Contains(c.Id, new StringEqualityExtension())).ToDictionary(x => x.Id, x => x.Breeds.SelectMany(b => CatBreedTemperamentHelper.GetTagTemperamentsList([b.Temperament])).DistinctBy(t => t.ToLower()));
    }

        public static IEnumerable<TagEntity> GetTagEntities(this Dictionary<string, IEnumerable<string>> catTagsDictionary)
    {
        return CatBreedTemperamentHelper.GetTagEntitiesFromTemperamentsList(catTagsDictionary.SelectMany(x => x.Value).DistinctBy(s => s.ToLower()).ToList());
    }
}
