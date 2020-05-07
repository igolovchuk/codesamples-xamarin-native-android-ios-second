using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using XamarinNativeDemo.Enums;

namespace XamarinNativeDemo.Providers
{
    public class SecurityProvider
    {
        public static string GetHashString(string project)
        {
            var hashByets = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(project));

            var builder = new StringBuilder();

            foreach (byte b in hashByets)
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }

        public static Dictionary<string, string> GetProjectHashes()
                                => Enum.GetNames(typeof(Projects)).ToDictionary(GetHashString, val => val);
    }
}
