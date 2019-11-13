using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itminus.LandLord.BlazorExtensions.SignalR.Patch
{
    public class HubMethodCallbackEx : IDisposable
    {
        private readonly HubConnectionEx _connection;
        private readonly Func<string[], Task> _callback;

        public HubMethodCallbackEx(string id, string methodName, HubConnectionEx connection, Func<string[], Task> callback)
        {
            this.MethodName = methodName;
            this.Id = id;
            this._connection = connection;
            this._callback = callback;
        }

        public string MethodName { [JSInvokable]get; private set; }
        public string Id { [JSInvokable]get; private set; }

        [JSInvokable]
        public Task On(string[] payloads) => this._callback(payloads);

        public void Dispose()
        {
            this._connection.RemoveHandle(this.MethodName, this.Id);
        }
    }
}
