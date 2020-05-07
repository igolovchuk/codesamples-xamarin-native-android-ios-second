using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinNativeDemo.Models;

namespace XamarinNativeDemo.Interfaces
{
    public interface ICarDataProvider
    {
        Task<AverageValueItem> Average(Vehicle vehicle);

        Task<List<NameValueItem>> GetMarkList();

        Task<List<NameValueItem>> GetModelList(string markId);

        Task<List<NameValueItem>> GetYearList();

        Task<List<NameValueItem>> GetGearBoxList();

        Task<List<NameValueItem>> GetEngineVolumeList();

        Task<List<NameValueItem>> GetFuelTypeList();
    }
}
