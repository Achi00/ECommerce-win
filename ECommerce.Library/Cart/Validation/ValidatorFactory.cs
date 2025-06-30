using ECommerce.Library.Cart;
using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

public class ValidatorFactory : IValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, Type> _interfaceCache = new();

    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICartValidator<T> GetValidator<T>() where T : IProduct
    {
        var interfaceType = GetValidatorInterfaceType(typeof(T));

        if (interfaceType != null)
        {
            return new ValidatorAdapter<T>(interfaceType, _serviceProvider);
        }

        return _serviceProvider.GetRequiredService<ICartValidator<T>>();
    }

    // based on correct interface we can use/access product type specific fields and values
    public void ValidateProduct(IProduct product, int quantity)
    {
        var productType = product.GetType();
        var interfaceType = GetValidatorInterfaceType(productType);

        if (interfaceType != null)
        {
            // generic method call to maintain type safety
            //var method = typeof(ValidatorFactory)
            //    .GetMethod(nameof(ValidateTyped), BindingFlags.NonPublic | BindingFlags.Instance)
            //    .MakeGenericMethod(interfaceType);

            var method = typeof(ValidatorFactory)
            .GetMethod(nameof(ValidateTyped), BindingFlags.NonPublic | BindingFlags.Instance)?
            .MakeGenericMethod(interfaceType);

            if (method == null)
            {
                throw new InvalidOperationException($"Method {nameof(ValidateTyped)} could not be found.");
            }

            method.Invoke(this, new object[] { product, quantity });
        }
        else
        {
            throw new InvalidOperationException($"No validator interface found for {productType.Name}");
        }
    }

    private void ValidateTyped<T>(IProduct product, int quantity) where T : IProduct
    {
        var validator = _serviceProvider.GetRequiredService<ICartValidator<T>>();
        validator.Validate((T)product, quantity);
    }

    private class ValidatorAdapter<T> : ICartValidator<T> where T : IProduct
    {
        private readonly Type _interfaceType;
        private readonly IServiceProvider _serviceProvider;

        public ValidatorAdapter(Type interfaceType, IServiceProvider serviceProvider)
        {
            _interfaceType = interfaceType;
            _serviceProvider = serviceProvider;
        }

        public bool Validate(T product, int quantity)
        {
            var validatorType = typeof(ICartValidator<>).MakeGenericType(_interfaceType);
            dynamic validator = _serviceProvider.GetRequiredService(validatorType);

            return validator.Validate(product, quantity);
        }
    }

    private Type GetValidatorInterfaceType(Type concreteType)
    {
        return _interfaceCache.GetOrAdd(concreteType, type =>
        {
            if (type.IsInterface && typeof(IProduct).IsAssignableFrom(type))
                return type;

            var productInterfaces = type.GetInterfaces()
                .Where(i => i != typeof(IProduct) && typeof(IProduct).IsAssignableFrom(i))
                .ToList();

            // in case we have more than one interface
            if (productInterfaces.Count > 1)
            {
                throw new InvalidOperationException(
                    $"Product {type.Name} implements multiple product interfaces: {string.Join(", ", productInterfaces.Select(i => i.Name))}. Please specify which validator to use.");
            }

            return productInterfaces.FirstOrDefault();
        });
    }
}