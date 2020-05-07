using System;
using System.IO;

namespace XamarinNativeDemo
{
    public class AppSettings
    {
        public static string StatisticFolderName => "StatisticData";

        public static string CacheFolderName => "CachedData";

        public static int CachedTimeInDays => 2;

//#if __ANDROID__
//        // Android-specific code
//        public static string BaseFolderName => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
//#else
//        // IOS-specific code
//        public static string BaseFolderName => Directory.GetCurrentDirectory();
//#endif
    }
}
