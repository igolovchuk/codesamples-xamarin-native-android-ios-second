using System;
using System.Threading.Tasks;
using XamarinNativeDemo.Interfaces;
using Newtonsoft.Json;

namespace XamarinNativeDemo.Providers
{
    public class JsonProvider : IJsonProvider
    {
        private JsonSerializerSettings _jsonSettings;

        public JsonProvider()
        {
            _jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public async Task<T> DeserializeAsync<T>(string value){
         
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(value, _jsonSettings));
        }

        public async Task<string> SerializeAsync(object value)
        {
            return await Task.Run(() => JsonConvert.SerializeObject(value, _jsonSettings));
        }
    }
}
