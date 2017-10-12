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
    public class LocalStorageReader : IStorageReader
    {
        public LocalStorageReader(string key)
        {
            _Key = key;
        }

        public Task<T> ReadAsync<T>()
        {
            if(!ApplicationData.Current.LocalSettings.Values.ContainsKey(_Key))
            {
                return Task.FromResult(default(T));
            }

            var data = ApplicationData.Current.LocalSettings.Values[_Key].ToString();
            if(string.IsNullOrWhiteSpace(data))
            {
                return Task.FromResult(default(T));
            }

            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(data)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return Task.FromResult((T)serializer.ReadObject(stream));
            }
        }

        private string _Key;
    }
}
