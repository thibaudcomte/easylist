using System.Threading.Tasks;

namespace EasyList.Proto.Core.Storage
{
    public interface IStorageReader
    {
        Task<T> ReadAsync<T>();
    }
}
