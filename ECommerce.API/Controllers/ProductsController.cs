﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using ECommerce.ProductCatalog.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCatalogService _service;

        public ProductsController()
        {
            var proxyFactory = new ServiceProxyFactory(
                _ => new FabricTransportServiceRemotingClientFactory());

            _service = proxyFactory.CreateServiceProxy<IProductCatalogService>(
                new System.Uri("fabric:/ECommerce/ECommerce.ProductCatalog"),
                new Microsoft.ServiceFabric.Services.Client.ServicePartitionKey(0));

        }

        // GET api/products
        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> Get()
        {
            var allProducts = await _service.GetAllProducts();

            return allProducts.Select(p => new ApiProduct
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.Availability > 0
            });
        }


        // POST api/products
        [HttpPost]
        public async Task Post([FromBody] ApiProduct product)
        {
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Availability = 100
            };

            await _service.AddProduct(newProduct);
        }
    }
}
