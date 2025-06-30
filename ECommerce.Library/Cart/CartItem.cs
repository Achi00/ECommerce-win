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

            // validates type, gaves interface and validates based on type
            _validatorFactory.ValidateProduct(product, quantity);
        }

        public void IncreaseQuantity(int amount = 1)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be positive");
            }
            // validate quantity of physical product based on stock
            // chack newest amount in cart
            int newTotalQuantity = Quantity + amount;
            _validatorFactory.ValidateProduct(Product, newTotalQuantity);
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
