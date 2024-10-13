using System.Net.Http.Headers;
using CatApi.Extensions;
using CatApi.Services.Interfaces;

namespace CatApi.Services;

public class HttpRequestService : IHttpRequestService
{
    private readonly HttpClient _httpClient;

    public HttpRequestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<string> HttpGet(Dictionary<string, string> DefaultHeaders, string url)
    {
        try
        {
            _httpClient.AddDefaultRequestHeaders(DefaultHeaders);
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException e)
        {
            throw new Exception($"The Api Source Provider responded with error message : '{e.Message}'");
        }

    }
}
