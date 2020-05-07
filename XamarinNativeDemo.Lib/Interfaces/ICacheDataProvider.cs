using System;
namespace XamarinNativeDemo.Interfaces
{
    public interface ICacheDataProvider
    {
        string Get(string fileName);
        void Set(string fileName, string content);
        void Remove(string fileName);
    }
}
