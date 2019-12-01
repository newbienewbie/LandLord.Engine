using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Itminus.LandLord.BlazorExtensions.SignalR.Patch
{
    internal static class JSRuntimeExtensions
    {
        public static T InvokeSync<T>(this IJSRuntime jsRuntime, string identifier, params object[] args)
        {
            if (jsRuntime == null)
                throw new ArgumentNullException(nameof(jsRuntime));

            if (jsRuntime is IJSInProcessRuntime inProcessJsRuntime)
            {
                return inProcessJsRuntime.Invoke<T>(identifier, args);
            }

            return jsRuntime.InvokeAsync<T>(identifier, args).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    public static class HubConnectionExtension
    {

        public static string Serialize<TObject>(this HubConnectionEx connection, TObject obj)
        {
            var result = JsonSerializer.Serialize(obj, obj.GetType(), connection.JsonSerializerOptions);
            return result;
        }

        public static TResult Deserialize<TResult>(this HubConnectionEx connection, string str) 
        {
            Console.WriteLine("deserialize json: "+ str);
            var result = JsonSerializer.Deserialize<TResult>(str, connection.JsonSerializerOptions );
            return result;
        }

        public static IDisposable On<TResult1>(this HubConnectionEx connection ,string methodName, Func<TResult1, Task> handler)
            => connection.On<TResult1, object, object, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1));

        public static IDisposable On<TResult1, TResult2>(this HubConnectionEx connection , string methodName, Func<TResult1, TResult2, Task> handler)
            => connection.On<TResult1, TResult2, object, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2));

        public static IDisposable On<TResult1, TResult2, TResult3>(this HubConnectionEx connection ,string methodName, Func<TResult1, TResult2, TResult3, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, object, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4>(this HubConnectionEx connection ,string methodName, Func<TResult1, TResult2, TResult3, TResult4, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, object, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5>(this HubConnectionEx connection ,string methodName, Func<TResult1, TResult2, TResult3, TResult4, TResult5, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, TResult5, object, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6>(this HubConnectionEx connection , string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, object, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7>(this HubConnectionEx connection ,string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, object, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8>(this HubConnectionEx connection , string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, object, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7, t8));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9>(this HubConnectionEx connection ,string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, Task> handler)
            => connection.On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, object>(methodName,
                (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => handler(t1, t2, t3, t4, t5, t6, t7, t8, t9));

        public static IDisposable On<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10>(this HubConnectionEx connection , string methodName,
            Func<TResult1, TResult2, TResult3, TResult4, TResult5, TResult6, TResult7, TResult8, TResult9, TResult10, Task> handler)
        {
            if (string.IsNullOrEmpty(methodName)) throw new ArgumentNullException(nameof(methodName));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            var callbackId = Guid.NewGuid().ToString();

            var callback = new HubMethodCallbackEx(callbackId, methodName, connection,
                 (payloads) =>
                 {
                     TResult1 t1 = default;
                     TResult2 t2 = default;
                     TResult3 t3 = default;
                     TResult4 t4 = default;
                     TResult5 t5 = default;
                     TResult6 t6 = default;
                     TResult7 t7 = default;
                     TResult8 t8 = default;
                     TResult9 t9 = default;
                     TResult10 t10 = default;

                     if (payloads.Length > 0)
                     {
                         t1 = connection.Deserialize<TResult1>(payloads[0]);
                     }
                     if (payloads.Length > 1)
                     {
                         t2 = connection.Deserialize<TResult2>(payloads[1]);
                     }
                     if (payloads.Length > 2)
                     {
                         t3 = connection.Deserialize<TResult3>(payloads[2]);
                     }
                     if (payloads.Length > 3)
                     {
                         t4 = connection.Deserialize<TResult4>(payloads[3]);
                     }
                     if (payloads.Length > 4)
                     {
                         t5 = connection.Deserialize<TResult5>(payloads[4]);
                     }
                     if (payloads.Length > 5)
                     {
                         t6 = connection.Deserialize<TResult6>(payloads[5]);
                     }
                     if (payloads.Length > 6)
                     {
                         t7 = connection.Deserialize<TResult7>(payloads[6]);
                     }
                     if (payloads.Length > 7)
                     {
                         t8 = connection.Deserialize<TResult8>(payloads[7]);
                     }
                     if (payloads.Length > 8)
                     {
                         t9 = connection.Deserialize<TResult9>(payloads[8]);
                     }
                     if (payloads.Length > 9)
                     {
                         t10 = connection.Deserialize<TResult10>(payloads[9]);
                     }

                     return handler(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                 }
            );

            connection.RegisterHandle(methodName, callback);

            return callback;
        }

    }
}
