using System;
namespace XamarinNativeDemo.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string s, char[] separators, string newVal)
        {
          string[] temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
          return string.Join(newVal, temp);
        }

        public static float ToFloat(this string value)
        {
            float.TryParse(value, out float number);

            return number;
        }
    }
}
