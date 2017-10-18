using System.Threading.Tasks;

namespace EasyList.Proto.Core.Storage
{
    public interface IStorageReaderWriter
    {
        Task<T> ReadAsync<T>();
        Task WriteAsync<T>(T value);
    }
}
