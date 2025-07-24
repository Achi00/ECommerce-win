using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Exceptions
{
    public class ProductStockException : Exception
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int AvailableStock { get; }
        public int AttemptedQuantity { get; }
        public string SourceOperation { get; }

        public ProductStockException(
            int productId,
            string productName,
            int availableStock,
            int attemptedQuantity,
            string sourceOperation
        ) : base($"Stock validation failed during '{sourceOperation}' for product '{productName}' (ID: {productId}). " +
                 $"Attempted: {attemptedQuantity}, Available: {availableStock}")
        {
            ProductId = productId;
            ProductName = productName;
            AvailableStock = availableStock;
            AttemptedQuantity = attemptedQuantity;
            SourceOperation = sourceOperation;
        }
    }
}
