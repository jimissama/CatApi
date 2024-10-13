namespace CatApi.Extensions;

public static class HttpClientExtensions
{
    public static void AddDefaultRequestHeaders(this HttpClient client, Dictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            if (!client.DefaultRequestHeaders.Contains(header.Key))
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
