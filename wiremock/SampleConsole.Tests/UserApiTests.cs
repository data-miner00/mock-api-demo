using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SampleConsole.Tests;

public record User(string Id, string FirstName, string LastName);

public class UserApiTests : IDisposable
{
    private readonly WireMockServer server = WireMockServer.Start();

    public UserApiTests()
    {
        this.server
            .Given(Request.Create().WithPath("/user").UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(new
                    {
                        id = "abc-123",
                        firstName = "John",
                        lastName = "Maverick",
                    })
            );
    }

    [Fact]
    public async Task RespondsWithTheUser()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(this.server.Url!) };

        var user = await httpClient.GetFromJsonAsync<User>("/user");

        Assert.Equal(new User("abc-123", "John", "Maverick"), user);
    }

    public void Dispose() => this.server.Stop();
}
