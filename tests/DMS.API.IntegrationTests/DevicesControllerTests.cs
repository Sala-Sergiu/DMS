using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DMS.API.IntegrationTests;

public class DevicesControllerTests : IClassFixture<DmsWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public DevicesControllerTests(DmsWebApplicationFactory factory)
    {
        factory.SeedDatabase();
        _client = factory.CreateClient();
    }

    private async Task AuthenticateAsync()
    {
        var token = await AuthHelper.GetTokenAsync(_client);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ── GET /api/devices ─────────────────────────────────────────────

    [Fact]
    public async Task GetDevices_WithoutAuth_Returns401()
    {
        var response = await _client.GetAsync("/api/devices");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetDevices_WithAuth_Returns200WithItems()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/devices");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Should().ContainKey("X-Total-Count");

        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);
        body.Should().NotBeNull();
        body!.Items.Should().NotBeEmpty();
        body.TotalCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetDevices_WithPagination_ReturnsCorrectPageSize()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/devices?pageNumber=1&pageSize=2");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);
        body!.Items.Should().HaveCount(2);
        body.PageSize.Should().Be(2);
        body.PageNumber.Should().Be(1);
    }

    // ── Search (Bonus) ───────────────────────────────────────────────

    [Fact]
    public async Task GetDevices_WithSearchTerm_ReturnsMatchingDevices()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/devices?searchTerm=Dell");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);
        body!.Items.Should().NotBeEmpty();
        body.Items.Should().AllSatisfy(d =>
            (d.Name.Contains("Dell", StringComparison.OrdinalIgnoreCase) ||
             d.Brand.Contains("Dell", StringComparison.OrdinalIgnoreCase))
            .Should().BeTrue());
    }

    [Fact]
    public async Task GetDevices_SearchIsCaseInsensitive()
    {
        await AuthenticateAsync();

        var upper = await _client.GetAsync("/api/devices?searchTerm=DELL");
        var lower = await _client.GetAsync("/api/devices?searchTerm=dell");

        var bodyUpper = await upper.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);
        var bodyLower = await lower.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);

        bodyUpper!.TotalCount.Should().Be(bodyLower!.TotalCount);
    }

    [Fact]
    public async Task GetDevices_WithUnknownSearchTerm_ReturnsEmpty()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/devices?searchTerm=xyznonexistent999");

        var body = await response.Content.ReadFromJsonAsync<PagedResult<DeviceDto>>(_json);
        body!.Items.Should().BeEmpty();
        body.TotalCount.Should().Be(0);
    }

    // ── GET /api/devices/{id} ────────────────────────────────────────

    [Fact]
    public async Task GetDeviceById_WithInvalidId_Returns404()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync($"/api/devices/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── POST /api/devices ────────────────────────────────────────────

    [Fact]
    public async Task CreateDevice_WithValidData_Returns201()
    {
        await AuthenticateAsync();

        var response = await _client.PostAsJsonAsync("/api/devices", new
        {
            name = "Test Laptop",
            serialNumber = "SN-TEST-001",
            assetTag = "AT-TEST-001",
            brand = "TestBrand",
            model = "TestModel X1",
            type = 1
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await response.Content.ReadFromJsonAsync<DeviceDto>(_json);
        body!.Name.Should().Be("Test Laptop");
    }

    [Fact]
    public async Task CreateDevice_WithMissingName_Returns400()
    {
        await AuthenticateAsync();

        var response = await _client.PostAsJsonAsync("/api/devices", new
        {
            name = "",
            serialNumber = "SN-TEST-002",
            assetTag = "AT-TEST-002",
            brand = "Brand",
            model = "Model",
            type = 1
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateDevice_WithDuplicateSerialNumber_Returns409()
    {
        await AuthenticateAsync();

        var response = await _client.PostAsJsonAsync("/api/devices", new
        {
            name = "Duplicate",
            serialNumber = "SN-001", // există în seed
            assetTag = "AT-UNIQUE-999",
            brand = "Brand",
            model = "Model",
            type = 1
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ── Auth ─────────────────────────────────────────────────────────

    [Fact]
    public async Task Login_WithInvalidCredentials_Returns401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "nobody@dms.com",
            password = "WrongPassword"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_Returns409()
    {
        await _client.PostAsJsonAsync("/api/auth/register", new
        {
            fullName = "First",
            email = "dup@dms.com",
            password = "Test@123456"
        });

        var response = await _client.PostAsJsonAsync("/api/auth/register", new
        {
            fullName = "Second",
            email = "dup@dms.com",
            password = "Test@123456"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    // ── Helpers ──────────────────────────────────────────────────────

    private sealed record PagedResult<T>(List<T> Items, int TotalCount, int PageNumber, int PageSize);
    private sealed record DeviceDto(Guid Id, string Name, string Brand, string Model, int Type, int Status);
}