using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Framework.Core.Utility
{
    public static class IdentityHelper
    {
        public static Guid NewSequentialGuid()
        {
            byte[] byteArray = Guid.NewGuid().ToByteArray();
            byte[] bytes = BitConverter.GetBytes(DateTime.Now.Ticks);
            byte[] b = new byte[byteArray.Length];
            b[0] = byteArray[0];
            b[1] = byteArray[1];
            b[2] = byteArray[2];
            b[3] = byteArray[3];
            b[4] = byteArray[4];
            b[5] = byteArray[5];
            b[6] = byteArray[6];
            b[7] = (byte)(192 | 15 & (int)byteArray[7]);
            b[9] = bytes[0];
            b[8] = bytes[1];
            b[15] = bytes[2];
            b[14] = bytes[3];
            b[13] = bytes[4];
            b[12] = bytes[5];
            b[11] = bytes[6];
            b[10] = bytes[7];
            return new Guid(b);
        }

        public static string Guid32
        {
            get
            {
                return IdentityHelper.NewSequentialGuid().ToString("N");
            }
        }

        public static string Guid16
        {
            get
            {
                return string.Format("{0:x}", (object)(((IEnumerable<byte>)Guid.NewGuid().ToByteArray()).Aggregate<byte, long>(1L, (Func<long, byte, long>)((current, b) => current * (long)((int)b + 1))) - DateTimeOffset.Now.Ticks));
            }
        }

        public static Guid CreateCombGuid()
        {
            byte[] byteArray = Guid.NewGuid().ToByteArray();
            DateTime dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse((Array)bytes1);
            Array.Reverse((Array)bytes2);
            Array.Copy((Array)bytes1, bytes1.Length - 2, (Array)byteArray, byteArray.Length - 6, 2);
            Array.Copy((Array)bytes2, bytes2.Length - 4, (Array)byteArray, byteArray.Length - 4, 4);
            return new Guid(byteArray);
        }
    }
}
