using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Library.Framework.Core.Extensions;
using Library.Framework.Core.Model;

namespace Library.Framework.Core.Utility
{
    public class MqRpcHelper
    {
        public delegate object Process(RpcDto message);
        public static RabbitMqHelper rabbitMqHelper;
        public MqRpcHelper(ServerConfiguration serverConfiguration)
        {
            //测试
            rabbitMqHelper = new RabbitMqHelper("amqp://192.168.137.2:5672/", "guest", "guest", 2);
            //正式
            //rabbitMqHelper = new RabbitMqHelper(serverConfiguration.Host + ":" + serverConfiguration.Port, serverConfiguration.User, serverConfiguration.Password, serverConfiguration.Model);
        }
        public static T CreateType<T>()
        {
            var type = typeof(T);
            TypeBuilder typeBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("sdfs"), AssemblyBuilderAccess.Run)
                .DefineDynamicModule(type.GetTypeInfo().Module.Name)
                .DefineType(type.FullName ?? throw new InvalidOperationException(), TypeAttributes.NotPublic);
            typeBuilder.AddInterfaceImplementation(typeof(T));
            MethodInfo[] methods = type.GetMethods();
            foreach (var m in methods)
            {
                ParameterInfo[] parameter = m.GetParameters();
                Type[] array = parameter.Select(p => p.ParameterType).ToArray();
                if (m.ReturnType == typeof(void))
                    throw new Exception("Rpc方法返回值不能为空！");
                MethodBuilder mbIm = typeBuilder.DefineMethod(m.Name,
                    MethodAttributes.Public | MethodAttributes.HideBySig |
                    MethodAttributes.NewSlot | MethodAttributes.Virtual |
                    MethodAttributes.Final,
                    m.ReturnType,
                    array);

                ILGenerator il = mbIm.GetILGenerator();

                LocalBuilder local = il.DeclareLocal(typeof(object[]));
                il.Emit(OpCodes.Ldc_I4, parameter.Length);
                il.Emit(OpCodes.Newarr, typeof(object));
                il.Emit(OpCodes.Stloc, local);
                il.Emit(OpCodes.Ldstr, type.FullName);
                il.Emit(OpCodes.Ldstr, m.Name);
                for (int i = 0; i < parameter.Length; i++)
                {
                    il.Emit(OpCodes.Ldloc, local);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldarg, i + 1);
                    Type t = array[i];
                    if (type.GetTypeInfo().IsValueType || type.IsGenericParameter)
                        il.Emit(OpCodes.Box, t);
                    il.Emit(OpCodes.Stelem_Ref);
                }
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Call, typeof(MqRpcHelper).GetMethod("ReadMessageViaRpc",
                                          new Type[] { typeof(string), typeof(string), typeof(object[]) }) ?? throw new InvalidOperationException());
                il.Emit(OpCodes.Ret);
                typeBuilder.DefineMethodOverride(mbIm, typeof(T).GetMethod(m.Name) ?? throw new InvalidOperationException());
            }
            Type personType = typeBuilder.CreateTypeInfo();
            return (T)Activator.CreateInstance(personType);
        }

        public static object ReadMessageViaRpc(string contract, string method, object[] objects)
        {
            var id = IdentityHelper.NewSequentialGuid().ToString("N");
            var s = new RpcDto
            {
                Id = id,
                Content = new RpcMessageClientDto
                {
                    Method = method,
                    Params = objects
                }
            };
            rabbitMqHelper.SendMessage($"server:{contract}", "", "", s.ObjectToBytes());
            object result = null;
            bool receive = false;

            rabbitMqHelper.ReadMessage(id, (ob, ea) =>
            {
                result = ea.Body.BytesToObject();
                receive = true;
                rabbitMqHelper.DeleteQueue(id);
            });
            var start = DateTime.Now.ToTimeStamp();
            while (!receive)
            {
                if (DateTime.Now.ToTimeStamp() - start > 3000)
                    throw new Exception("等待超时");
            }
            return result;
        }

        public static void RegisterRpcServer(string name, Process process)
        {
            //RabbitMqHelper rabbitMqHelper = new RabbitMqHelper("amqp://192.168.137.2:5672/", "guest", "guest", 2);
            rabbitMqHelper.ReadMessage($"server:{name}", (ob, ea) =>
            {
                var r = ea.Body.BytesToObject<RpcDto>();
                var s = process(r);
                rabbitMqHelper.SendMessage(r.Id, "", "", s.ObjectToBytes());
            });
        }

    }
}
