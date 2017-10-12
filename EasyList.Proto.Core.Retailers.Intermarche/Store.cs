using System.Diagnostics;

namespace EasyList.Proto.Core.Retailers.Intermarche
{
    [DebuggerDisplay("{Id} {Name} {City}")]
    public class Store : IStore
    {
        public Retailer Retailer { get; set; }
        IRetailer IStore.Retailer => Retailer;
        public string Name { get; internal set; }
        public int Id { get; internal set; }
        public int Idt { get; internal set; }
        public string Address { get; internal set; }
        public string City { get; internal set; }
        public string ZipCode { get; internal set; }
        public int Latitude { get; internal set; }
        public int Longitude { get; internal set; }
        public string Urlh { get; internal set; }

        public string CartUrl => "https://drive.intermarche.com/mon-panier";
    }
}
