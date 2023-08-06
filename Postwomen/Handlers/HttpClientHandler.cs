using System.Net.Http.Headers;

namespace Postwomen.Handlers;

public class HttpLoggingHandler : DelegatingHandler
{
    public HttpLoggingHandler(HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
    {

    }
    async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var req = request;
        var id = Guid.NewGuid().ToString();
        var msg = $"[{id} -   Request]";

        Console.WriteLine($"{msg}========Start==========");
		Console.WriteLine($"{msg} {req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
		Console.WriteLine($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

        foreach (var header in req.Headers)
			Console.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

        if (req.Content != null)
        {
            foreach (var header in req.Content.Headers)
				Console.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (req.Content is StringContent || this.IsTextBasedContentType(req.Headers) || this.IsTextBasedContentType(req.Content.Headers))
            {
                var result = await req.Content.ReadAsStringAsync();

				Console.WriteLine($"{msg} Content:");
				Console.WriteLine($"{msg} {string.Join("", result.Cast<char>().Take(255))}...");

            }
        }

        DateTime start = DateTime.Now;
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        DateTime end = DateTime.Now;

		Console.WriteLine($"{msg} Duration: {end - start}");
		Console.WriteLine($"{msg}==========End==========");

        msg = $"[{id} - Response]";
		Console.WriteLine($"{msg}=========Start=========");

        var resp = response;

		Console.WriteLine($"{msg} {req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

        foreach (var header in resp.Headers)
			Console.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

        if (resp.Content != null)
        {
            foreach (var header in resp.Content.Headers)
				Console.WriteLine($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

            if (resp.Content is StringContent || this.IsTextBasedContentType(resp.Headers) || this.IsTextBasedContentType(resp.Content.Headers))
            {
                start = DateTime.Now;
                var result = await resp.Content.ReadAsStringAsync();
                end = DateTime.Now;

				Console.WriteLine($"{msg} Content:");
				Console.WriteLine($"{msg} {string.Join("", result.Cast<char>().Take(255))}...");
				Console.WriteLine($"{msg} Duration: {end - start}");
            }
        }

		Console.WriteLine($"{msg}==========End==========");
        return response;
    }

    readonly string[] types = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

    bool IsTextBasedContentType(HttpHeaders headers)
    {
		if (!headers.TryGetValues("Content-Type", out IEnumerable<string> values))
			return false;
		var header = string.Join(" ", values).ToLowerInvariant();

        return types.Any(t => header.Contains(t));
    }
}