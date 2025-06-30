using ECommerce.Library.Products;
using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Cart
{
    public interface ICartValidator<T> where T : IProduct
    {
        bool Validate(T product, int quantity);
    }
}
