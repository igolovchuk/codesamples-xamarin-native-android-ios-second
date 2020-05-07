using System;
using System.IO;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Interfaces;

namespace XamarinNativeDemo.Providers
{
    public class FileSystemProvider : IFileSystemProvider
    {
        private string _folderPrefix;

        public FileSystemProvider(OperationSystem operationSystem)
        {
            _folderPrefix = GetBasePathAccordingSystem(operationSystem);
        }

        public void AddToPath(string destinationPath = null)
        {
            if(!string.IsNullOrEmpty(destinationPath)){
                _folderPrefix = Path.Combine(_folderPrefix, destinationPath);
            }
           
            if(!Directory.Exists(_folderPrefix)){
                Directory.CreateDirectory(_folderPrefix);
            }
        }

        public string ReadFile(string path)
        {
            var filePath = Path.Combine(_folderPrefix, path);
                               
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }

            return string.Empty;
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles($"{_folderPrefix}/{path}");
        }

        public void SaveFile(string path, string content)
        {
            File.WriteAllText($"{_folderPrefix}/{path}", content);
        }

        public void AppendToFile(string path, string content)
        {
            File.AppendAllText($"{_folderPrefix}/{path}", content);
        }

        public void DeleteFile(string path)
        {
            var filePath = Path.Combine($"{_folderPrefix}/{path}");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public DateTime CreationTime(string path){
            var filePath = Path.Combine(_folderPrefix, path);

            if (File.Exists(filePath))
            {
                return File.GetCreationTime($"{_folderPrefix}/{path}");
            }

            return DateTime.MinValue;
        }

        public string Combine(string path)
        {
            return Path.Combine(_folderPrefix, path);
        }

        private string GetBasePathAccordingSystem(OperationSystem operationSystem)
        {
            var result = string.Empty;

            switch (operationSystem)
            {
                case OperationSystem.Android:
                    result = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    break;
                case OperationSystem.Ios:
                    result = Directory.GetCurrentDirectory();
                    break;
            }

            return result;
        }
    }
}
