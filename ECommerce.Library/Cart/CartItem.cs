using ECommerce.Library.Cart.Validation;
using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Cart
{
    public class CartItem
    {
        public IProduct Product { get; }
        public int Quantity { get; private set; }
        private readonly IValidatorFactory _validatorFactory;

        public CartItem(IProduct product, int quantity, IValidatorFactory validatorFactory)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be at least 1");
            }

            if (product is null)
            {
                throw new InvalidOperationException("Product is missing");
            }

            Product = product;
            Quantity = quantity;
            _validatorFactory = validatorFactory;

            // Validate using the appropriate validator
            ValidateProduct(product, quantity);
        }

        private void ValidateProduct(IProduct product, int quantity)
        {
            // resolve the correct generic method at runtime
            // validate type
            dynamic dynamicProduct = product;
            _validatorFactory.ValidateProduct(dynamicProduct, quantity);
        }


        public void IncreaseQuantity(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amound must be positive");
            }
            // validate quantity if physical product
            int newTotalQuantity = Quantity + amount;
            ValidateProduct(Product, newTotalQuantity);
            Quantity = newTotalQuantity;
        }

        public void DecreaseQuantity(int amount = 1)
        {
            if (amount <= 0 || amount > Quantity)
            {
                throw new ArgumentException("Invalid decrease amount, must be positive and less that \"Quantyty\"");
            }
            Quantity -= amount;
        }

        public decimal TotalPrice() => Product.Price() * Quantity;
    }

}
