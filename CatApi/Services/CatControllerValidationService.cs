using System.Net;
using System.Resources;
using CatApi.Models.Response;
using CatApi.Services.Interfaces;

namespace CatApi.Services;

public class CatControllerValidationService : ICatControllerValidationService
{

    private readonly ResourceManager _resourceManager;

    public CatControllerValidationService(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    public void GetCatByIdInvalidId(int id)
    {
        if (id < 1)
            throw new HttpRequestException(_resourceManager.GetString("IdGreaterZero"), null, HttpStatusCode.BadRequest);
    }

    public void GetCatsInvalidPageAndPageSize(int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            throw new HttpRequestException(_resourceManager.GetString("PageAndPageSizeGreaterZero"), null, HttpStatusCode.BadRequest);
    }

    public void GetCatByIdNull(CatResponse? catResponse)
    {
        if (catResponse is null)
            throw new HttpRequestException(_resourceManager.GetString("CatNotFound"), null, HttpStatusCode.NotFound);
    }
}
