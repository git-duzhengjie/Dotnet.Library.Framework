using System.Collections.Generic;

namespace Library.Framework.Core.Utility
{
    public class SingletonUtility
    {
        private static Dictionary<string, object> _singleton = new Dictionary<string, object>();

        public static void AddSingleton<T>(T t) {
            _singleton.Add(typeof(T).FullName, t);
        }

        public static T GetSingleton<T>() {
            object obj;
            _singleton.TryGetValue(typeof(T).FullName, out obj);
            if (obj == null)
                throw new System.Exception($"实体{typeof(T).Name}还未注册！");
            return (T)obj;
        }
    }
}
