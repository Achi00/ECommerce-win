
namespace ECommerce.Library.Shipping
{
    public interface IShippingService
    {
        string GetShippingAddress(int id);
        void Ship(string destination);
    }
}
