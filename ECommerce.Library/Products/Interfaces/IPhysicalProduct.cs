﻿namespace ECommerce.Library.Products.Interfaces
{
    public interface IPhysicalProduct : IProduct
    {
        int Stock();
        double WeightInKg { get; }
    }
}