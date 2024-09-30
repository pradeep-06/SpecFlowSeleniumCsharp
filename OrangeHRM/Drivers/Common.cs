using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace OrangeHRM.Drivers
{
    public class Common
    {
        public static ISpecFlowOutputHelper _specFlowOutputHelper;

        public Common(ISpecFlowOutputHelper specFlowOutputHelper) {
            _specFlowOutputHelper = specFlowOutputHelper;
        }

        public static string GetProjectPath() { 
            string currentDirectory = Directory.GetCurrentDirectory();
            string projectPath = Path.GetFullPath(currentDirectory, "..\\..\\..");
            return projectPath;
        }

        public static string GetEnvValue() {
            string envparameter = null;
            string explorepath =GetProjectPath();
            StreamReader reader = new StreamReader(explorepath + "\\..\\parameters.txt");
            try {
                while (!reader.EndOfStream) { 
                 var line = reader.ReadLine();
                    var values = line.Split(',');
                    envparameter = values[0].Trim();
                }
                return envparameter;
            } catch {
                string failure = "Environment value not set";
                _specFlowOutputHelper.WriteLine(failure);
                return failure;
            }
        }

        public static string GetBrowserValue()
        {
            string browsername = null;
            string explorepath = GetProjectPath();
            StreamReader reader = new StreamReader(explorepath + "\\..\\parameters.txt");
            try
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    browsername = values[1].Trim();
                }
                return browsername;
            }
            catch
            {
                string failure = "Browser value not set";
                _specFlowOutputHelper.WriteLine(failure);
                return failure;
            }
        }

        public static string passwordEncrypt(string plaintext, string key) {
            byte[] iv = new byte[16];
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter((Stream)cryptoStream)) { 
                          writer.Write(plaintext);
                        }
                        array=memoryStream.ToArray();
                    }


                }
            }
            _specFlowOutputHelper.WriteLine(Convert.ToBase64String(array));
            return Convert.ToBase64String(array);
        }
        public static string GenerateRandomKey(int size) {
            using (var rng = new RNGCryptoServiceProvider()) { 
                byte[] key = new byte[size];
                rng.GetBytes(key);
                _specFlowOutputHelper.WriteLine(Convert.ToBase64String(key));
                return Convert.ToBase64String(key);
            }
        }


    }
}
