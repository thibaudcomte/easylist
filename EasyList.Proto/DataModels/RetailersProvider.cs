using EasyList.Proto.Core.Retailers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.DataModels
{
    class RetailersProvider : IRetailersProvider
    {
        public List<IRetailer> Retailers { get; } = new List<IRetailer>();
        IEnumerable<IRetailer> IRetailersProvider.Retailers => Retailers;
    }
}
