using ECommerce.Library;
using ECommerce.Library.Cart;
using ECommerce.Library.Cart.Controllers;
using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products;
using ECommerce.Library.Products.Interfaces;
using ECommerce.Library.Shipping;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

var services = new ServiceCollection();

//register validators
services.AddScoped<ICartValidator<IPhysicalProduct>, PhysicalProductCartValidator>();
services.AddScoped<ICartValidator<IDigitalProduct>, DigitalProductCartValidator>();

// register factory
services.AddScoped<IValidatorFactory, ValidatorFactory>();

// register cart controller
services.AddScoped<CartController>();

var serviceProvider = services.BuildServiceProvider();
var cartService = serviceProvider.GetRequiredService<CartController>();


IPhysicalProduct laptop = new PhysicalProduct(
        id: 1,
        name: "Laptop",
        imageUrl: "laptop.jpg",
        price: 999.99m,
        description: "High-end laptop",
        stock: 10,
        weightInKg: 5
);
IPhysicalProduct pc = new PhysicalProduct(
        id: 2,
        name: "Pc",
        imageUrl: "pc.jpg",
        price: 1200,
        description: "office pc",
        stock: 3,
         weightInKg: 15
);

IPhysicalProduct gamingPc = new PhysicalProduct(
        id: 2,
        name: "Pc",
        imageUrl: "pc.jpg",
        price: 2500,
        description: "High-end gaming pc",
        stock: 1,
        weightInKg: 20
);

IDigitalProduct windows = new DigitalProduct(
            id: 3,
            name: "Windows",
            imageUrl: "img.jog",
            price: 120,
            description: "windows 10 installer",
            userEmail: "email@gmail.com",
            downloadLink: "https://download.com",
            fileSize: 25
);

try
{
    cartService.AddProductToCart(laptop, 2);
    cartService.AddProductToCart(pc, 3);
    cartService.AddProductToCart(gamingPc, 1);

    /* 
     * if we already add exxact amount in cart as stocka and still increment it, this will throw error
     */
    cartService.IncreaseItemQuantity(pc);
    Console.WriteLine("Physical product added successfully!");

    //// This will call your DigitalProductCartValidator  
    //cartService.AddProductToCart(windows, 1);
    //Console.WriteLine("Digital product added successfully!");
    //cartService.AddProductToCart(laptop, 1);
    //cartService.AddProductToCart(windows, 2);


    // This will throw exception from PhysicalProductCartValidator
    //Console.WriteLine(physicalProduct.GetStock());
    //cartService.AddProductToCart(physicalProduct, 8); // More than stock!

    //Console.WriteLine(cartService.);
    //var digitalItems = cartService.GetDigitalProducts();

    var physicalProducts = cartService.GetProductsWithQuantities<IPhysicalProduct>();


    foreach (var (item, quantity) in physicalProducts)
    {
        Console.WriteLine($"{item.Name()}: ${item.Price()} - {quantity}");
    }

    // Load from file
    var json = File.ReadAllText("shippingRules.json");

    var options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };
    var cityShippingRules = JsonSerializer.Deserialize<List<CityShippingRules>>(json, options);



    ShippingCalculator shippingCalculator = new ShippingCalculator(cityShippingRules);
    var shippingCost = shippingCalculator.CalculateShippingCost(City.Tbilisi, physicalProducts);

    Console.WriteLine($"Sipping cost: ${shippingCost}");

    //var a = cartService.CartItems();
    //foreach (var item in a)
    //{
    //    Console.WriteLine(item.Product.Name() + " - " + item.Quantity);
    //}
    //cartService.IncreaseItemQuantity(windows);

    //cartService.IncreaseItemQuantity(windows);

    //cartService.DecreaseItemQuantity(pc);

    //var removedItem = cartService.RemoveProductFromCart(1);

    //if (removedItem != null)
    //{
    //    Console.WriteLine($"Removed item: {removedItem.ProductId}, {removedItem.Name}, ${removedItem.Price}");
    //}

    Console.WriteLine("-------------");

    //foreach (var item in a)
    //{
    //    Console.WriteLine(item.Product.Name() + " - " + item.Quantity);
    //}

    //var physical = cartService.GetPhysicalProducts();

    //var removed = cartService.RemoveProductFromCart(2);

    //Console.WriteLine($"Item {removed.Name()} Removed succesfully");

}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
}