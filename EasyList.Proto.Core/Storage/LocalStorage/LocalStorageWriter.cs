using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace EasyList.Proto.Core.Storage.LocalStorage
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
