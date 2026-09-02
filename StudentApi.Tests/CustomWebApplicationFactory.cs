using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SchoolManagementASPBackend.Tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(
                    new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:SchoolDatabase"] =
                            "Server=(localdb)\\MSSQLLocalDB;Database=SchoolManagementTestDB;Trusted_Connection=True;TrustServerCertificate=True;"
                    });
            });

            return base.CreateHost(builder);
        }
    }
}