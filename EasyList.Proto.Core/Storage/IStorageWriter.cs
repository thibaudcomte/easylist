using System.Threading.Tasks;

namespace EasyList.Proto.Core.Storage
{
    public interface IStorageWriter
    {
        Task WriteAsync<T>(T value);
    }
}
