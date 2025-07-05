using ECommerce.Library.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Library.Shipping
{
    public class ShippingCalculator
    {
        // store record data in dictionary for O(1) lookup
        private readonly Dictionary<City, CityShippingRules> _shippingRules;
        public ShippingCalculator(IEnumerable<CityShippingRules> shippingRules)
        {
            _shippingRules = shippingRules.ToDictionary(rule => rule.City);
            ValidateAllCitiesHaveRules();
        }

        // shipping cost is only allowed to be calculates for IPhysicalProduct or PhysicalProduct
        // digital products are not shipped!
        public double CalculateShippingCost(City destination, IEnumerable<(IPhysicalProduct Product, int Quantity)> productsWithQuantities)
        {
            // if destination city exists
            if (!_shippingRules.TryGetValue(destination, out var rules))
            {
                throw new InvalidOperationException($"{destination} is not a valid shipping destination.");
            }

            // total weight
            var totalWeight = productsWithQuantities.Sum(item => item.Product.WeightInKg * item.Quantity);

            //cost based on distance
            double distanceCost = rules.distanceKilometer * rules.CostPerKilometer;

            // cost based on weight
            double weightCost = totalWeight * rules.CostPerKilogram;
            // get total weight * item quantity of items in cart
            //double totalWeightBasedOnQuantity = productsWithQuantities.Sum(item => item.Product.WeightInKg * item.Quantity);

            //double totalCostOfKilometerBasedOnWeight;

            // final cost
            var shippingCost = distanceCost + weightCost;

            return shippingCost;
        }


        private void ValidateAllCitiesHaveRules()
        {
            // get all cities from the enum
            var allCities = Enum.GetValues<City>();

            // get cities that have shipping rules
            var citiesWithRules = _shippingRules.Keys;

            // find cities that are missing shipping rules
            var missingCities = allCities.Except(citiesWithRules).ToList();

            if (missingCities.Any())
            {
                throw new InvalidOperationException($"Missing shipping rules for cities: {string.Join(", ", missingCities)}");
            }
        }
    }

    public record CityShippingRules(City City, int BaseCost, double CostPerKilometer, double CostPerKilogram, int distanceKilometer);
}
