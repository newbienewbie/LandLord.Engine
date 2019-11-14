﻿


using Blazor.Extensions;
using JsonSubTypes;
using LandLord.Shared;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itminus.LandLord.BlazorExtensions.SignalR.Patch
{
    public class HubConnectionEx : IDisposable
    {
        private const string ON_METHOD = "BlazorExtensions.SignalR.On";
        private const string ON_CLOSE_METHOD = "BlazorExtensions.SignalR.OnClose";
        private const string OFF_METHOD = "BlazorExtensions.SignalR.Off";
        private const string CREATE_CONNECTION_METHOD = "BlazorExtensions.SignalR.CreateConnection";
        private const string REMOVE_CONNECTION_METHOD = "BlazorExtensions.SignalR.RemoveConnection";
        private const string START_CONNECTION_METHOD = "BlazorExtensions.SignalR.StartConnection";
        private const string STOP_CONNECTION_METHOD = "BlazorExtensions.SignalR.StopConnection";
        private const string INVOKE_ASYNC_METHOD = "BlazorExtensions.SignalR.InvokeAsync";
        private const string INVOKE_WITH_RESULT_ASYNC_METHOD = "BlazorExtensions.SignalR.InvokeWithResultAsync";

        internal HttpConnectionOptions Options { get; }
        internal string InternalConnectionId { get; }
        public IJSRuntime Runtime { get; set; }

        private Dictionary<string, Dictionary<string, HubMethodCallbackEx>> callbacks = new Dictionary<string, Dictionary<string, HubMethodCallbackEx>>();

        private HubCloseCallback closeCallback;
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        public HubConnectionEx(IJSRuntime runtime, HttpConnectionOptions options)
        {
            this.Runtime = runtime;
            this.Options = options;
            this.InternalConnectionId = Guid.NewGuid().ToString();
            runtime.InvokeSync<object>(CREATE_CONNECTION_METHOD,
                this.InternalConnectionId,
                DotNetObjectReference.Create(this.Options));
            this.JsonSerializerSettings = GetJsonSerializerSettings();
        }
        private JsonSerializerSettings GetJsonSerializerSettings()
        {
            var playerCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayerCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .RegisterSubtype(typeof(Shadowed), PlayerCardKind.Shadowed)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var playingCardJsonConverter = JsonSubtypesConverterBuilder
                .Of(typeof(PlayingCard), "Kind") // type property is only defined here
                .RegisterSubtype(typeof(NormalCard), PlayerCardKind.NormalCard)
                .RegisterSubtype(typeof(BlackJokerCard), PlayerCardKind.BlackJokerCard)
                .RegisterSubtype(typeof(RedJokerCard), PlayerCardKind.RedJokerCard)
                .SerializeDiscriminatorProperty() // ask to serialize the type property
                .Build();
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(playerCardJsonConverter);
            settings.Converters.Add(playingCardJsonConverter);
            return settings;
        }


        public ValueTask<object> StartAsync() => this.Runtime.InvokeAsync<object>(START_CONNECTION_METHOD, this.InternalConnectionId);
        public ValueTask<object> StopAsync() => this.Runtime.InvokeAsync<object>(STOP_CONNECTION_METHOD, this.InternalConnectionId);

        internal void RegisterHandle(string methodName, HubMethodCallbackEx callback)
        {
            if (this.callbacks.TryGetValue(methodName, out var methodHandlers))
            {
                methodHandlers[callback.Id] = callback;
            }
            else
            {
                this.callbacks[methodName] = new Dictionary<string, HubMethodCallbackEx>
                {
                    { callback.Id, callback }
                };
            }

            this.Runtime.InvokeSync<object>(ON_METHOD, this.InternalConnectionId, DotNetObjectReference.Create(callback));
        }

        internal void RemoveHandle(string methodName, string callbackId)
        {
            if (this.callbacks.TryGetValue(methodName, out var callbacks))
            {
                if (callbacks.TryGetValue(callbackId, out var callback))
                {
                    this.Runtime.InvokeSync<object>(OFF_METHOD, this.InternalConnectionId, methodName, callbackId);
                    //HubConnectionManager.Off(this.InternalConnectionId, handle.Item1);
                    callbacks.Remove(callbackId);

                    if (callbacks.Count == 0)
                    {
                        this.callbacks.Remove(methodName);
                    }
                }
            }
        }

        public void OnClose(Func<Exception, Task> callback)
        {
            this.closeCallback = new HubCloseCallback(callback);
            this.Runtime.InvokeSync<object>(ON_CLOSE_METHOD,
                this.InternalConnectionId,
                DotNetObjectReference.Create(this.closeCallback));
        }

        public ValueTask<object> InvokeAsync(string methodName, params object[] args) =>
            this.Runtime.InvokeAsync<object>(INVOKE_ASYNC_METHOD, this.InternalConnectionId, methodName, args);

        public ValueTask<TResult> InvokeAsync<TResult>(string methodName, params object[] args) =>
            this.Runtime.InvokeAsync<TResult>(INVOKE_WITH_RESULT_ASYNC_METHOD, this.InternalConnectionId, methodName, args);

        public void Dispose() => this.Runtime.InvokeSync<object>(REMOVE_CONNECTION_METHOD, this.InternalConnectionId);
    }
}