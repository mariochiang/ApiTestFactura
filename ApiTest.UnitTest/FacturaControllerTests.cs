using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ApiTest.UnitTests
{
    public class FacturaControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public FacturaControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        // Prueba para obtener todas las facturas
        [Fact]
        public async Task GetInvoices_ShouldReturnOk()
        {
            // Arrange
            var response = await _client.GetAsync("/api/factura");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        }

        // Prueba para obtener las facturas agrupadas por comuna
        [Fact]
        public async Task GetInvoicesGroupedByComuna_ShouldReturnGroupedInvoices()
        {
            // Arrange
            var response = await _client.GetAsync("/api/factura/grouped-by-comuna");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Prueba para obtener las facturas de una comuna específica
        [Fact]
        public async Task GetInvoicesByComuna_ShouldReturnFilteredInvoices()
        {
            // Arrange
            var comuna = 1; 
            var response = await _client.GetAsync($"/api/factura/{comuna}");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Prueba para obtener la lista de compradores con el monto total de compras realizadas
        [Fact]
        public async Task GetCompradoresConMontoTotal_ShouldReturnValidData()
        {
            // Arrange
            var response = await _client.GetAsync("/api/factura/compradores");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode); 
        }

        // Prueba para obtener facturas con monto total por comprador
        [Fact]
        public async Task GetFacturasPorComprador_ShouldReturnValidData()
        {
            // Arrange
            var response = await _client.GetAsync("/api/factura/facturas-por-comprador");

            // Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
