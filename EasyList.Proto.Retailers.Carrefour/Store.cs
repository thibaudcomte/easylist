using EasyList.Proto.Core.Retailers;
using System.Runtime.Serialization;

namespace EasyList.Proto.Retailers.Carrefour
{
    [DataContract]
    public class Store : IStore
    {
        public Retailer Retailer { get; set; }
        IRetailer IStore.Retailer => Retailer;

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }

        public string CartUrl => "https://courses-en-ligne.carrefour.fr";
    }
}
