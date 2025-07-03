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


IPhysicalProduct laptop = new PhysicalProduct(
        id: 1,
        name: "Laptop",
        imageUrl: "laptop.jpg",
        price: 999.99m,
        description: "High-end laptop",
        stock: 10
);
IPhysicalProduct pc = new PhysicalProduct(
        id: 2,
        name: "Pc",
        imageUrl: "pc.jpg",
        price: 1200,
        description: "High-end gaming pc",
        stock: 3
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
    // This will call your PhysicalProductCartValidator
    cartService.AddProductToCart(laptop, 2);
    cartService.AddProductToCart(pc, 3);
    Console.WriteLine("Physical product added successfully!");

    // This will call your DigitalProductCartValidator  
    cartService.AddProductToCart(windows, 1);
    Console.WriteLine("Digital product added successfully!");
    cartService.AddProductToCart(laptop, 1);
    cartService.AddProductToCart(windows, 2);


    // This will throw exception from PhysicalProductCartValidator
    //Console.WriteLine(physicalProduct.GetStock());
    //cartService.AddProductToCart(physicalProduct, 8); // More than stock!

    //Console.WriteLine(cartService.);
    var digitalItems = cartService.GetDigitalProducts();

    var a = cartService.CartItems();
    foreach (var item in a)
    {
        Console.WriteLine(item.Product.Name() + " - " + item.Quantity);
    }
    cartService.IncreaseItemQuantity(windows);

    cartService.IncreaseItemQuantity(windows);

    cartService.DecreaseItemQuantity(pc);
    cartService.DecreaseItemQuantity(pc);
    //cartService.DecreaseItemQuantity(pc);

    cartService.RemoveProductFromCart(1);


    Console.WriteLine("-------------");

    foreach (var item in a)
    {
        Console.WriteLine(item.Product.Name() + " - " + item.Quantity);
    }

    //var physical = cartService.GetPhysicalProducts();

    //var removed = cartService.RemoveProductFromCart(2);

    //Console.WriteLine($"Item {removed.Name()} Removed succesfully");

}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
}