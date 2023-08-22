// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT License.

namespace Orleans.ShoppingCart.Silo.StartupTasks;

public sealed class SeedProductStoreTask : IStartupTask
{
    private readonly IGrainFactory _grainFactory;
    private readonly ILogger<SeedProductStoreTask> _logger;

    public SeedProductStoreTask(
        IGrainFactory grainFactory,
        ILogger<SeedProductStoreTask> logger)
    {
        _grainFactory = grainFactory;
        _logger = logger;
    }

    async Task IStartupTask.Execute(CancellationToken cancellationToken)
    {
        _logger.LogWarning("Begin seeding product store...");
        var faker = new ProductDetails().GetBogusFaker();

        foreach (var product in faker.GenerateLazy(50))
        {
            var productGrain = _grainFactory.GetGrain<IProductGrain>(product.Id);
            await productGrain.CreateOrUpdateProductAsync(product);
            _logger.LogWarning($"Seeded product id {product.Id}.");
        }
        _logger.LogWarning("Finished seeding product store.");
    }
}
