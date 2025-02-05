﻿using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    class ServiceFabricProductRepository : IProductRepository
    {
        private readonly IReliableStateManager _stateManager;

        public ServiceFabricProductRepository(IReliableStateManager stateManager)
        {
            _stateManager = stateManager;
        }

        public async Task AddProduct(Product product)
        {
            var products = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(tx, product.Id, product, (id, value) => product);

                await tx.CommitAsync();
            }
        }

        public async Task<Product[]> GetAllProducts()
        {
            var products = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");

            var result = new List<Product>();

            using (ITransaction tx = _stateManager.CreateTransaction())
            {

                var allProducts = await products.CreateEnumerableAsync(tx, EnumerationMode.Unordered);

                using (var enumerator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var current = enumerator.Current;
                        result.Add(current.Value);
                    }
                }
            }
            return result.ToArray();
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var products = await _stateManager
                .GetOrAddAsync<IReliableDictionary<Guid, Product>>("products");


            using (ITransaction tx = _stateManager.CreateTransaction())
            {
                var result = await products.TryGetValueAsync(tx, productId);
                return result.HasValue ? result.Value : null;
            }
        }
    }
}
