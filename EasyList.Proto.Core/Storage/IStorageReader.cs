using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Storage
{
    public interface IStorageReader
    {
        Task<T> ReadAsync<T>();
    }
}
