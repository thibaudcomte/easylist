using System.Collections.Generic;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailersProvider
    {
        IEnumerable<IRetailer> Retailers { get; }
    }
}
