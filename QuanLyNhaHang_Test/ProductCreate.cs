using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using QuanLyNhaHang.Models.QLNHModels;

namespace QuanLyNhaHang_Test
{
    public class ProductIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            var newProduct = new
            {
                Id = 100,
                Name = "mon3",
                Price = 25000,
                Stock = 20,
                
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/api/Product/Create", content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<Product>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(createdProduct);
            Assert.Equal("mon3", createdProduct.Name);
            Assert.Equal(25000, createdProduct.Price);
        }
    }

}
