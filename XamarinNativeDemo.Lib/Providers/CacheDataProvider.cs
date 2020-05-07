using System;
using System.IO;
using XamarinNativeDemo.Interfaces;

namespace XamarinNativeDemo.Providers
{
    public class CacheDataProvider : ICacheDataProvider
    {
        private IFileSystemProvider _fileSystemProvider;

        public CacheDataProvider(IFileSystemProvider fileSystemProvider)
        {
            _fileSystemProvider = fileSystemProvider;
            _fileSystemProvider.AddToPath(AppSettings.CacheFolderName);
        }

        public string Get(string fileName){
            string result = null;

            if(!string.IsNullOrEmpty(fileName))
            {
                if (_fileSystemProvider.CreationTime($"{fileName}.json")
                                   .AddDays(AppSettings.CachedTimeInDays) >= DateTime.Now)
                {
                    result = _fileSystemProvider.ReadFile($"{fileName}.json");
                }
                else
                {
                    Remove(fileName);
                } 
            }

            return result;
        }

        public void Set(string fileName, string content){
            _fileSystemProvider.SaveFile($"{fileName}.json", content);
        }

        public void Remove(string fileName){
            _fileSystemProvider.DeleteFile($"{fileName}.json");
        }
    }
}
