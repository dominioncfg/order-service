using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderService.Api.FunctionalTests.SeedWork;

public static class HttpClientExtensions
{
    public static async Task PostAndExpectBadRequestAsync(this HttpClient client, string url, object request)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(url, httpContent);
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public static async Task PostAndExpectCreatedAsync(this HttpClient client, string url, object request)
    {
        var json = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync(url, httpContent);
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
