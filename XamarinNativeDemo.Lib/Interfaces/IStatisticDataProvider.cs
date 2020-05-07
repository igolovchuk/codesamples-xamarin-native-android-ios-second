using System;
using XamarinNativeDemo.Enums;

namespace XamarinNativeDemo.Interfaces
{
    public interface IStatisticDataProvider
    {
        void SendAsync(Projects project, object record);
    }
}
