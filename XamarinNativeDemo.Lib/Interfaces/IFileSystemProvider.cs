using System;
namespace XamarinNativeDemo.Interfaces
{
    public interface IFileSystemProvider
    {
        string ReadFile(string path);
        void DeleteFile(string path);
        string[] GetFiles(string path);
        string Combine(string path);
        void SaveFile(string path, string content);
        void AddToPath(string destinationPath = null);
        DateTime CreationTime(string path);
        void AppendToFile(string path, string content);
    }
}
