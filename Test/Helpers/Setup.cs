using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Dominio.Interfaces;
using Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;

namespace Test.Helpers
{
    [TestClass]
    public class Setup
    {
        public const string PORT = "5001";
        public static WebApplicationFactory<Program> Factory { get; private set; } = default!;
        public static HttpClient Client { get; private set; } = default!;

        // MSTest injeta o TestContext como parâmetro, não precisa ser propriedade estática
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext testContext)
        {
            Factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("https_port", PORT)
                           .UseEnvironment("Testing");

                    builder.ConfigureServices(services =>
                    {
                        // Substitui serviço real por mock nos testes
                        services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
                    });
                });

            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri($"https://localhost:{PORT}")
            });
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Client?.Dispose();
            Factory?.Dispose();
        }
    }
}
