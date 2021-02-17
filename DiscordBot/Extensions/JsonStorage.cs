using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot
{
    internal static class JsonStorage
    {
        /// <summary>
        /// Restores object from file
        /// </summary>
        /// <param name="path">File path</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RestoreObject<T>(string path)
        {
            var json = string.Empty;

            using (var fs = File.OpenRead(path))
            {
                using (var sr = new StreamReader(fs, new System.Text.UTF8Encoding(false)))
                {
                    json = sr.ReadToEnd();
                }
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Restores object from file
        /// </summary>
        /// <param name="path">File path</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> RestoreObjectAsync<T>(string path)
        {
            var json = string.Empty;

            using (var fs = File.OpenRead(path))
            {
                using (var sr = new StreamReader(fs, new System.Text.UTF8Encoding(false)))
                {
                    json = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Stores object to file
        /// </summary>
        /// <param name="obj">Object to store</param>
        /// <param name="path">File path</param>
        public static void StoreObject(object obj, string path)
        {
            var json = JsonConvert.SerializeObject(obj);
            using (var fs = File.OpenWrite(path))
            {
                using (var sr = new StreamWriter(fs, new System.Text.UTF8Encoding(false)))
                {
                    sr.WriteAsync(json).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Stores object to file
        /// </summary>
        /// <param name="obj">Object to store</param>
        /// <param name="path">File path</param>
        public static async Task StoreObjectAsync(object obj, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var json = JsonConvert.SerializeObject(obj);
            using (var fs = File.OpenWrite(path))
            {
                using (var sr = new StreamWriter(fs, new System.Text.UTF8Encoding(false)))
                {
                    await sr.WriteAsync(json).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Returns value from file using key
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="key">Key that keeps value</param>
        /// <returns></returns>
        public static string GetValue(string path, string key)
        {
            var data = RestoreObject<dynamic>(path);
            var dic = data.ToObject<Dictionary<string, string>>();
            return dic.ContainsKey(key) ? dic[key] : "";
        }

        /// <summary>
        /// Returns value from file using it's position
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="pos">Position</param>
        /// <returns></returns>
        public static string GetValue(string path, int pos)
        {
            var data = RestoreObject<dynamic>(path);
            var list = data.ToObject<List<string>>();
            return list[pos];
        }

        /// <summary>
        /// Stores value to file using key
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void SetValue(string path, string key, string value)
        {
            var data = RestoreObject<dynamic>(path); ;
            var obj = data.ToObject<Dictionary<string, string>>();
            if (obj.ContainsKey(key))
            {
                obj[key] = value;
                StoreObject(obj, path);
            }
            else return;
        }

        /// <summary>
        /// Returns key:value count from file
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        public static int GetCount(string path)
        {
            var data = RestoreObject<dynamic>(path);
            var list = data.ToObject<List<string>>();
            return list.Count;
        }
    }
}