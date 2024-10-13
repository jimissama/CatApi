using CatApi.Models.Response;

namespace CatApi.Services.Interfaces;

public interface ICatControllerValidationService
{
    public void GetCatByIdInvalidId(int id);

    public void GetCatsInvalidPageAndPageSize(int page, int pageSize);
    
    public void GetCatByIdNull(CatResponse? catResponse);
}
