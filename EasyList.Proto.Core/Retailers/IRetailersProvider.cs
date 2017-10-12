using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Retailers
{
    public interface IRetailersProvider
    {
        IEnumerable<IRetailer> Retailers { get; }
    }
}
