using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Library.Framework.Core.Extensions
{
    public static class UtilityExtensions
    {
        /// <summary>
        /// 将实体类序列化为JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string SerializeJson<T>(this T data)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 反序列化JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeJson<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static DateTime ToDateTime(this long timestamp)
        {
            return TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local).AddMilliseconds((double) timestamp);
        }

        public static long ToTimeStamp(this DateTime time)
        {
            DateTime localTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (long)(time - localTime).TotalMilliseconds;
        }

        public static T BytesToObject<T>(this byte[] bytes)
        {
            using (MemoryStream stream= new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }

        public static object BytesToObject(this byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        public static byte[] ObjectToBytes(this object ob)
        {
            if (ob == null)
                return new byte[] { };
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, ob);
                return stream.GetBuffer();
            }
        }

    }
}
