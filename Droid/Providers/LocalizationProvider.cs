using Android.App;
using XamarinNativeDemo.Interfaces;

namespace XamarinNativeDemo.Droid.Providers
{
    public struct LocalizationProvider
    {
        public static string Translate(string key)
        {
          var result = key;

          var resourceField = typeof(Resource.String).GetField(key);
              if(resourceField != null){
                  int resID = (int)resourceField.GetValue(null);
                  result = Application.Context.Resources.GetText(resID);
              }

          return result;
        }

        public static string Translate(int resID)
        {
            return Application.Context.Resources.GetText(resID);
        }
    }
}
