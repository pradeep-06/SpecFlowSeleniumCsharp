using AventStack.ExtentReports.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrangeHRM.Drivers
{
    public class Configuration
    {
        static string configjson = File.ReadAllText(Common.GetProjectPath() + "\\Config.json");
        public static JToken Config() {
            var config = JObject.Parse(configjson);
            return config;
        }
        public static string getBit16Key() {
            var config = Config();
            string value = (string)config.SelectToken("$.16BitKey");
            return value;

        }

        public static string getValue(string key) { 
            var config =Config();
            string value = (string)config.SelectToken("$." + key);
            return value;
        }

        public static string getDeCryptPassword(String password) {
            var config = Config();
            string value = (string)config.SelectToken("$." + password);
            return deCryptPassword(password, getBit16Key());
        }

        private static string deCryptPassword(string password, string key) {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(password);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                ICryptoTransform decryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                        
                    }
                }
            }          
        }

    }
}
