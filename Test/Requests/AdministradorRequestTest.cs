using System;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;
using MinimalApi.DTOs;
using MinimalApi.Dominio.ModelViews; // aqui ficam LoginDTO e AdministradorLogado

namespace Test.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            Setup.AssemblyInit(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Setup.AssemblyCleanup();
        }
        
        [TestMethod]
        public async Task TestarGetSetPropriedades()
        {
            var loginDTO = new LoginDTO
            {
                Email = "adm@teste.com",
                Senha = "123456"
            };

            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
            var response = await Setup.Client.PostAsync("/administradores/login", content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsFalse(string.IsNullOrWhiteSpace(admLogado?.Email));
            Assert.IsFalse(string.IsNullOrWhiteSpace(admLogado?.Perfil));
            Assert.IsFalse(string.IsNullOrWhiteSpace(admLogado?.Token));

            Console.WriteLine(admLogado?.Token);
        }
    }
}
