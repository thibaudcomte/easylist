using EasyList.Proto.Core.Storage;
using System.Threading.Tasks;

namespace EasyList.Proto.Core.Uwp.Storage.LocalStorage
{
    public class LocalStorageReaderWriter : IStorageReaderWriter
    {
        private readonly LocalStorageReader _Reader;
        private readonly LocalStorageWriter _Writer;

        public LocalStorageReaderWriter(string key)
        {
            _Reader = new LocalStorageReader(key);
            _Writer = new LocalStorageWriter(key);
        }

        public Task<T> ReadAsync<T>()
        {
            return _Reader.ReadAsync<T>();
        }

        public Task WriteAsync<T>(T value)
        {
            return _Writer.WriteAsync(value);
        }
    }
}
