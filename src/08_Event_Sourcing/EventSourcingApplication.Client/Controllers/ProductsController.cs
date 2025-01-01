using EventSourcingApplication.Client.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using EventSourcingApplication.Shared.Events;
using EventSourcingApplication.Shared.Models;
using EventSourcingApplication.Shared.Services.Abstractions;

namespace EventSourcingApplication.Client.Controllers;

public class ProductsController 
    (IEventStoreService eventStoreService, IMongoDbService mongoDbService)
    : Controller
{
     public async Task<IActionResult> Index()
    {
        var productCollection = mongoDbService.GetCollection<Shared.Models.Product>("Products");
        var products = await (await productCollection.FindAsync(_ => true)).ToListAsync();
        return View(products);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVM model)
    {
        NewProductAddedEvent newProductAddedEvent = new()
        {
            ProductId = Guid.NewGuid().ToString(),
            InitialCount = model.Count,
            InitialPrice = model.Price,
            IsAvailable = model.IsAvailable,
            ProductName = model.ProductName
        };

        await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(newProductAddedEvent) });

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(string productId)
    {
        var productCollection = mongoDbService.GetCollection<Product>("Products");
        var product = await (await productCollection.FindAsync(p => p.Id.ToString() == productId)).FirstOrDefaultAsync();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> CountUpdate(Product model, int durum)
    {
        var productCollection = mongoDbService.GetCollection<Shared.Models.Product>("Products");
        var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

        if (durum == 1)
        {
            CountDecreasedEvent countDecreasedEvent = new()
            {
                ProductId = model.Id.ToString(),
                DecrementAmount = model.Count,
            };
            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(countDecreasedEvent) });
        }
        else if (durum == 0)
        {
            CountIncreasedEvent countIncreasedEvent = new()
            {
                ProductId = model.Id.ToString(),
                IncrementAmount = model.Count,
            };
            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(countIncreasedEvent) });
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PriceUpdate(Product model, int durum)
    {
        var productCollection = mongoDbService.GetCollection<Product>("Products");
        var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

        if (durum == 1)
        {
            PriceDecreasedEvent priceDecreasedEvent = new()
            {
                ProductId = model.Id.ToString(),
                DecrementAmount = model.Price
            };
            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(priceDecreasedEvent) });
        }
        else if (durum == 0)
        {
            PriceIncreasedEvent priceIncreasedEvent = new()
            {
                ProductId = model.Id.ToString(),
                IncrementAmount = model.Price
            };
            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(priceIncreasedEvent) });
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AvailableUpdate(Product model)
    {
        var productCollection = mongoDbService.GetCollection<Product>("Products");
        var product = await (await productCollection.FindAsync(p => p.Id == model.Id)).FirstOrDefaultAsync();

        if (product.IsAvailable != model.IsAvailable)
        {
            AvailabilityChangedEvent availabilityChangedEvent = new()
            {
                ProductId = model.Id.ToString(),
                IsAvailable = model.IsAvailable,
            };
            await eventStoreService.AppendToStreamAsync("products-stream", new[] { eventStoreService.GenerateEventData(availabilityChangedEvent) });
        }
        return RedirectToAction("Index");
    }
}