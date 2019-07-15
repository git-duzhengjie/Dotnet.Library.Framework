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
        public T CreateType<T>()
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
                il.Emit(OpCodes.Ldstr, type.Name);
                il.Emit(OpCodes.Ldstr, m.Name);
                //il.Emit(OpCodes.Ldtoken, m.ReturnType);
                //il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
                il.Emit(OpCodes.Call, typeof(T).GetMethod("ReadMessageViaRpc",
                                          new Type[] { typeof(object[]), typeof(string), typeof(Type) }) ?? throw new InvalidOperationException());
                il.Emit(OpCodes.Ret);
                typeBuilder.DefineMethodOverride(mbIm, typeof(T).GetMethod(m.Name) ?? throw new InvalidOperationException());
            }
            Type personType = typeBuilder.CreateTypeInfo();
            return (T)Activator.CreateInstance(personType);
        }

        public static object ReadMessageViaRpc(object[] objects, string contract, string method)
        {
            RabbitMqHelper rabbitMqHelper = new RabbitMqHelper("amqp://192.168.137.2:5672/", "guest", "guest", 2);
            var id = IdentityHelper.NewSequentialGuid().ToString("N");
            rabbitMqHelper.SendMessage($"server:{contract}", "", "", new RpcDto
            {
                Id = id,
                Content = new RpcMessageClientDto
                {
                    Method = method,
                    Params = objects
                }
            }.ObjectToBytes());
            object result = null;
            bool receive=false;

            rabbitMqHelper.ReadMessage(id, (ob, ea) =>
            {
                result = ea.Body.BytesToObject();
                receive = true;
            });
            var start = DateTime.Now.ToTimeStamp();
            while (!receive)
            {
               if(DateTime.Now.ToTimeStamp()-start>300)
                    throw new Exception("等待超时");
            }
            return result;
        }

        public static void RegisterRpcServer(string name)
        {
            RabbitMqHelper rabbitMqHelper = new RabbitMqHelper("amqp://192.168.137.2:5672/", "guest", "guest", 2);
            rabbitMqHelper.ReadMessage($"server:{name}", (ob, ea) =>
            {
                var r = ea.Body.BytesToObject<RpcDto>();

            });
        }

    }
}
