using ECommerce.Library.Products;
using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Cart.Validation
{
    public class DigitalProductCartValidator : ICartValidator<IDigitalProduct>
    {
        public bool Validate(IDigitalProduct product, int quantity)
        {
            return true;
        }

        public bool Validate(IDigitalProduct product, int quantity, string source)
        {
            return true;
        }
    }
}
