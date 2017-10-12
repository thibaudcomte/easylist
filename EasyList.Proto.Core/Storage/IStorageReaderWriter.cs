using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Storage
{
    public interface IStorageReaderWriter
    {
        Task<T> ReadAsync<T>();
        Task WriteAsync<T>(T value);
    }
}
