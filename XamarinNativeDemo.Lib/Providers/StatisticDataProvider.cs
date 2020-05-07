using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Models;
using Newtonsoft.Json;

namespace XamarinNativeDemo.Providers
{
    public class StatisticDataProvider : IStatisticDataProvider
    {
        private HttpClient _httpClient;
        private INetworkProvider _networkProvider;
        private IFileSystemProvider _fileSystemProvider;
        private IJsonProvider _jsonProvider;
        private IDeviceInfoProvider _deviceInfoProvider;

        public StatisticDataProvider(HttpClient httpClient,
                                     INetworkProvider networkProvider,
                                     IFileSystemProvider fileSystemProvider,
                                     IJsonProvider jsonProvider,
                                     IDeviceInfoProvider deviceInfoProvider)
        {
            _httpClient = httpClient;
            _networkProvider = networkProvider;
            _jsonProvider = jsonProvider;
            _deviceInfoProvider = deviceInfoProvider;
            _fileSystemProvider = fileSystemProvider;
            _fileSystemProvider.AddToPath(AppSettings.StatisticFolderName);
        }

        public void SendAsync(Projects project, object record){
            Task.Run(async () =>
            {
                try
                {
                    var deviceInfo = _deviceInfoProvider.GetDeviceInfo();
                    var model = new PandoraModel
                    {
                        Identifier = $"{deviceInfo.Id}{SecurityProvider.GetHashString(project.ToString())}",
                        Record = new Record { DeviceInfo = deviceInfo, Data = record }
                    };

                    var modelData = await _jsonProvider.SerializeAsync(model);

                    // If there is network connection we try to send statistic data.
                    if (_networkProvider.IsConnected())
                    {
                        // Trying to get data that wasn't sent to the server by some reason.
                        var notSendData = _fileSystemProvider.ReadFile(project.ToString());

                        var data = modelData;
                        if (!string.IsNullOrEmpty(notSendData))
                        {
                            data = modelData.Insert(0, notSendData);
                        }

                        var content = new StringContent(data, Encoding.UTF8, "application/json");
                        var result = await _httpClient.PostAsync(Constants.ApiStatisticBaseUrl, content);

                        if (result.IsSuccessStatusCode)
                        {
                            _fileSystemProvider.DeleteFile(project.ToString());
                        }
                        else
                        {
                            _fileSystemProvider.AppendToFile(project.ToString(), modelData);
                        }
                    }
                    else {
                        _fileSystemProvider.AppendToFile(project.ToString(), modelData);
                    }
                }
                catch
                {
                    // Ignore any exception.
                }
            });
        }
    }
}
