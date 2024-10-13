namespace CatApi.Services.Interfaces;

public interface IHttpRequestService
{
    public Task<string> HttpGetAsync(Dictionary<string, string> DefaultHeaders, string url);
}
