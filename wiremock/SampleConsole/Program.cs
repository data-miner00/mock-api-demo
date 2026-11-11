using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

// Random port is used
using var server = WireMockServer.Start();

server
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

using var httpClient = new HttpClient
{
    BaseAddress = new Uri(server.Url!),
};

var response = await httpClient.GetAsync("/user");
var body = await response.Content.ReadAsStringAsync();

Console.WriteLine($"GET {response.RequestMessage?.RequestUri} -> {(int)response.StatusCode}");
Console.WriteLine(body);
