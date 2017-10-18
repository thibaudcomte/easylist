using EasyList.Proto.Core.Storage;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace EasyList.Proto.Core.Uwp.Storage.LocalStorage
{
    public class LocalStorageWriter : IStorageWriter
    {
        public LocalStorageWriter(string key)
        {
            _Key = key;
        }

        public async Task WriteAsync<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, value);
                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    ApplicationData.Current.LocalSettings.Values[_Key] = await streamReader.ReadToEndAsync();
                }
            }
        }

        private string _Key;
    }
}
