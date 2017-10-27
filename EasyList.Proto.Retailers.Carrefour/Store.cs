using EasyList.Proto.Core.Retailers;

namespace EasyList.Proto.Retailers.Carrefour
{
    public class Store : IStore
    {
        public Retailer Retailer { get; }
        IRetailer IStore.Retailer => Retailer;
        public int Id { get; }
        public string Name { get; }
        public string Address { get; }
        public string City { get; }
        public string ZipCode { get; }

        public string CartUrl => "https://courses-en-ligne.carrefour.fr";

        public Store(Retailer retailer, int id, string name, string address, string city, string zipCode)
        {
            Retailer = retailer;
            Id = id;
            Name = name;
            Address = address;
            City = city;
            ZipCode = zipCode;
        }
    }
}
