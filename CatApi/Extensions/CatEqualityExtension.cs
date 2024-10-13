using System.Diagnostics.CodeAnalysis;
using CatApi.Models.Entities;

namespace CatApi.Extensions;

public class CatEqualityExtension : IEqualityComparer<CatEntity>
{
    public bool Equals(CatEntity? x, CatEntity? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.CatId == y.CatId;
    }

    public int GetHashCode([DisallowNull] CatEntity obj)
    {
        if (obj is null) return 0;
        int hashId = obj.CatId == null ? 0 : obj.CatId.GetHashCode();
        return hashId;
    }
}
