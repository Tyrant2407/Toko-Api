using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using TokoApi;

namespace TokoApi.Tests.Infrastructure;

/// <summary>
/// Boots a real ASP.NET Core test server with an ephemeral MySQL Testcontainer.
/// The database is migrated automatically and torn down after all tests in the collection complete.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mysqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase("toko_test")
        .WithUsername("root")
        .WithPassword("testpassword")
        .WithCleanUp(true)
        .Build();

    // ── IAsyncLifetime ────────────────────────────────────────────────────────

    public async Task InitializeAsync()
    {
        // Start the real MySQL docker container
        await _mysqlContainer.StartAsync();

        // Run EF Core migrations on the fresh empty container
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mysqlContainer.DisposeAsync();
    }

    // ── WebApplicationFactory override ────────────────────────────────────────

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove the production AppDbContext registration
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();

            // Register AppDbContext pointing to the Testcontainer's connection string
            var connectionString = _mysqlContainer.GetConnectionString();
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

            // EF Core migrations are now run in InitializeAsync after the host is fully built,
            // to avoid intermediate BuildServiceProvider logger-freeze issues.
        });
    }

    /// <summary>
    /// Creates an HttpClient that bypasses JWT authentication for integration tests.
    /// We inject a fake Admin token via override in each test as needed.
    /// </summary>
    public HttpClient CreateClientWithAuth(string role = "Admin")
    {
        var client = CreateClient();
        var token = JwtTestHelper.GenerateTestToken(role);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>
    /// Resolves a scoped service from the test server's DI container.
    /// </summary>
    public T GetRequiredService<T>() where T : notnull
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
