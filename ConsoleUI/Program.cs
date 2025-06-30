using ECommerce.Library.Cart;
using ECommerce.Library.Cart.Controllers;
using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products;
using ECommerce.Library.Products.Interfaces;
using Microsoft.Extensions.DependencyInjection;

// set up DI
var services = new ServiceCollection();

//register validators
services.AddScoped<ICartValidator<IPhysicalProduct>, PhysicalProductCartValidator>();
services.AddScoped<ICartValidator<IDigitalProduct>, DigitalProductCartValidator>();

// register factory
services.AddScoped<IValidatorFactory, ValidatorFactory>();

// Register cart controller
services.AddScoped<CartController>();

var serviceProvider = services.BuildServiceProvider();
var cartService = serviceProvider.GetRequiredService<CartController>();


IPhysicalProduct physicalProduct = new PhysicalProduct(
        id: 1,
        name: "Laptop",
        imageUrl: "laptop.jpg",
        price: 999.99m,
        description: "High-end laptop",
        stock: 10
);

IDigitalProduct digitalProduct = new DigitalProduct(
            id: 1,
            name: "Windows 10",
            imageUrl: "img.jog",
            price: 120,
            description: "windows 10 installer",
            userEmail: "email@gmail.com",
            downloadLink: "https://download.com",
            fileSize: 25
);

try
{
    // This will call your PhysicalProductCartValidator
    cartService.AddProductToCart(physicalProduct, 2);
    Console.WriteLine("Physical product added successfully!");

    // This will call your DigitalProductCartValidator  
    cartService.AddProductToCart(digitalProduct, 1);
    Console.WriteLine("Digital product added successfully!");

    // This will throw exception from PhysicalProductCartValidator
    //Console.WriteLine(physicalProduct.GetStock());
    cartService.AddProductToCart(physicalProduct, 8); // More than stock!

    var items = cartService.GetDigitalProducts();

    var physical = cartService.GetPhysicalProducts();

    foreach (var item in items)
    {
        Console.WriteLine(item.GetType());
        Console.WriteLine(item.FileSize());
    }

    foreach (var item in physical)
    {
        Console.WriteLine(item.GetType());
        Console.WriteLine(item.Stock());
    }

    //var cart = cartService.

}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
}