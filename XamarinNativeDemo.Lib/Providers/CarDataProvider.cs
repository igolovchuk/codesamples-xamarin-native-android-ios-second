using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Models;
using XamarinNativeDemo.Enums;
using System.Text;
using System.Globalization;

namespace XamarinNativeDemo.Providers
{
    public class CarDataProvider : ICarDataProvider
    {
        private HttpClient _httpClient;
        private IJsonProvider _jsonProvider;
        private ICacheDataProvider _cacheDataProvider;
        private INetworkProvider _networkProvider;
        private IStatisticDataProvider _statisticDataProvider;

        public CarDataProvider(IJsonProvider jsonProvider, ICacheDataProvider cacheDataProvider, HttpClient httpClient, INetworkProvider networkProvider, IStatisticDataProvider statisticDataProvider)
        {
            _httpClient = httpClient; 
            _jsonProvider = jsonProvider;
            _cacheDataProvider = cacheDataProvider;
            _networkProvider = networkProvider;
            _statisticDataProvider = statisticDataProvider;
        }

        public async Task<List<NameValueItem>> GetMarkList()
        {
            return await GetDataAsync<List<NameValueItem>>(Constants.KeyMarks, "/categories/1/marks");
        }

        public async Task<List<NameValueItem>> GetModelList(string markId)
        {
            return await GetDataAsync<List<NameValueItem>>($"{Constants.KeyModels}{markId}", $"/categories/1/marks/{markId}/models");
        }

        public async Task<List<NameValueItem>> GetGearBoxList()
        {
            return await GetDataAsync<List<NameValueItem>>(Constants.KeyGearBox, "/categories/1/gearboxes");
        }

        public async Task<List<NameValueItem>> GetFuelTypeList()
        {
            return await GetDataAsync<List<NameValueItem>>(Constants.KeyFuelType, "/type");
        }

        public async Task<AverageValueItem> Average(Vehicle vehicle)
        {
            var paramDictionary = new Dictionary<string, string>{
                { "marka_id", vehicle.Id },
                { "model_id", vehicle.ModelId },
                { "yers", vehicle.Year },
                { "gear_id", vehicle.GearId },
                { "fuel_id", vehicle.FuelId },
                { "engineVolume", vehicle.EngineVolume },
                { "custom", vehicle.IsNotLegalized },
                { "api_key", Constants.ApiAutoKey }
            };

            //string url = $"/average_price?main_category=1&marka_id={vehicle.Id}&model_id={vehicle.ModelId}&yers={vehicle.Year}&gear_id={vehicle.GearId}&fuel_id={vehicle.FuelId}&engineVolume={vehicle.EngineVolume}&custom={vehicle.IsNotLegalized}&api_key={Constants.ApiAutoKey}";
           // var result = await _cachedClient.GetStringAsync(url);
            //var result = "{\"total\":3,\"arithmeticMean\":4766.666666666667,\"interQuartileMean\":4600,\"percentiles\":{\"1.0\":4600,\"5.0\":4600,\"25.0\":4600,\"50.0\":4600,\"75.0\":4850,\"95.0\":5050,\"99.0\":5090},\"prices\":[5100,4600,4600],\"classifieds\":[17567128,21064324,20604580]}";
           // var result = "{\"total\":0,\"arithmeticMean\":null,\"interQuartileMean\":null,\"percentiles\":{\"1.0\":\"NaN\",\"5.0\":\"NaN\",\"25.0\":\"NaN\",\"50.0\":\"NaN\",\"75.0\":\"NaN\",\"95.0\":\"NaN\",\"99.0\":\"NaN\"},\"prices\":[],\"classifieds\":[]}";
            return await GetDataAsync<AverageValueItem>(string.Empty, BuildUrl("/average_price?main_category=1", paramDictionary));
        }

        public async Task<List<NameValueItem>> GetYearList()
        {
            return await Task.Run(() =>
            {
                var resultList = new List<NameValueItem>();

                for (var year = DateTime.Now.Year; year >= 1900; year--)
                {
                    resultList.Add(new NameValueItem{ Name = year.ToString(), Value = year.ToString() });
                }

                return resultList;
            });
        }

        public async Task<List<NameValueItem>> GetEngineVolumeList()
        {
            return await Task.Run(() =>
            {
                var resultList = new List<NameValueItem>();

                for (decimal volume = 0.5m; volume <= 7m; volume += 0.1m)
                {
                    resultList.Add(new NameValueItem { Name = volume.ToString(CultureInfo.InvariantCulture), Value = volume.ToString(CultureInfo.InvariantCulture) });
                }

                return resultList;
            });
        }

        private async Task<T> GetDataAsync<T>(string cacheKey, string url) where T : class 
        {
            T result = null;
            var cacheData = _cacheDataProvider.Get(cacheKey);

            try
            {
                if (string.IsNullOrEmpty(cacheData))
                {
                    if(_networkProvider.IsConnected())
                    {
                        var requestUrl = url.Contains(Constants.ApiAutoKey)
                     ? $"{Constants.ApiAutoBaseUrl}{url}"
                     : $"{Constants.ApiAutoBaseUrl}{url}?api_key={Constants.ApiAutoKey}";

                        var response = await _httpClient.GetAsync(requestUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            cacheData = await response.Content.ReadAsStringAsync();
                            _cacheDataProvider.Set(cacheKey, cacheData);

                            result = await _jsonProvider.DeserializeAsync<T>(cacheData);
                        }
                        else
                        {
                            _statisticDataProvider.SendAsync(Projects.AutoLogs, response);
                        }
                    }
                }
                else {
                    result = await _jsonProvider.DeserializeAsync<T>(cacheData);
                }
            }
            catch (Exception ex)
            {
                _statisticDataProvider.SendAsync(Projects.AutoLogs, ex);
            }

            return result;
        }

        private string BuildUrl(string baseUrl, Dictionary<string, string> parameters)
        {
            var builder = new StringBuilder(baseUrl);

            foreach(var parameter in parameters){
                if(!parameter.Value.Equals(Constants.NoParamValue))
                {
                    if(parameter.Value.Contains(Constants.SeparatorDash)){
                        var values = parameter.Value.Split(new[] { Constants.SeparatorDash }, StringSplitOptions.None);
                        if(values.Length == 2){
                            builder.Append($"&{parameter.Key}={values[0]}&{parameter.Key}={values[1]}");
                        }
                    }
                    else {
                        builder.Append($"&{parameter.Key}={parameter.Value}");
                    }

                }
            }

            return builder.ToString();
        }
    }
}
