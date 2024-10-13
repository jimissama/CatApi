namespace CatApi.Services.Interfaces;

public interface IHttpRequestService
{
    public Task<string> HttpGet(Dictionary<string, string> DefaultHeaders, string url);
}
