using System;
using System.Threading.Tasks;

namespace XamarinNativeDemo.Interfaces
{
    public interface IJsonProvider
    {
        Task<T> DeserializeAsync<T>(string value);

        Task<string> SerializeAsync(object value);
    }
}
