using EasyList.Proto.Core.Retailers;
using System.Collections.Generic;

namespace EasyList.Proto.DataModels
{
    class RetailersProvider : IRetailersProvider
    {
        public List<IRetailer> Retailers { get; } = new List<IRetailer>();
        IEnumerable<IRetailer> IRetailersProvider.Retailers => Retailers;
    }
}
