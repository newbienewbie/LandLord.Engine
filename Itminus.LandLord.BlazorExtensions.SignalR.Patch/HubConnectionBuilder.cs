﻿using Blazor.Extensions;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itminus.LandLord.BlazorExtensions.SignalR.Patch
{
    public class HubConnectionBuilderEx
    {
        private bool _hubConnectionBuilt;

        private IJSRuntime _runtime;

        internal HttpConnectionOptions Options { get; set; } = new HttpConnectionOptions();

        public HubConnectionBuilderEx(IJSRuntime runtime)
        {
            this._runtime = runtime;
        }

        /// <summary>
        /// Build a SignalR <see cref="HubConnection"/>
        /// This method can only be called once.
        /// </summary>
        /// <returns>Return a <see cref="HubConnection"/>.</returns>
        public HubConnectionEx Build()
        {
            // Build can only be used once
            if (this._hubConnectionBuilt)
            {
                throw new InvalidOperationException("HubConnectionBuilder allows creation only of a single instance of HubConnection.");
            }

            this._hubConnectionBuilt = true;

            return new HubConnectionEx(this._runtime, this.Options);
        }
    }

    /// <summary>
    /// Extension methods for <see cref="IHubConnectionBuilder"/>.
    /// </summary>
    public static class HubConnectionBuilderHttpExtensions
    {
        /// <summary>
        /// Configures the <see cref="HubConnection" /> to use HTTP-based transports to connect to the specified URL.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder" /> to configure.</param>
        /// <param name="url">The URL the <see cref="HttpConnection"/> will use.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilderEx WithUrl(this HubConnectionBuilderEx hubConnectionBuilder, string url, Action<HttpConnectionOptions> configureHttpOptions = null)
        {
            if (hubConnectionBuilder == null) throw new ArgumentNullException(nameof(hubConnectionBuilder));
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            hubConnectionBuilder.Options.Url = url;
            configureHttpOptions?.Invoke(hubConnectionBuilder.Options);
            return hubConnectionBuilder;
        }

        /// <summary>
        /// Enables the MessagePack protocol for SignalR.
        /// </summary>
        /// <param name="hubConnectionBuilder">The <see cref="HubConnectionBuilder"/> representing the SignalR server to add MessagePack protocol support to.</param>
        /// <returns>The same instance of the <see cref="HubConnectionBuilder"/> for chaining.</returns>
        public static HubConnectionBuilderEx AddMessagePackProtocol(this HubConnectionBuilderEx hubConnectionBuilder)
        {
            if (hubConnectionBuilder == null) throw new ArgumentNullException(nameof(hubConnectionBuilder));
            hubConnectionBuilder.Options.EnableMessagePack = true;
            return hubConnectionBuilder;
        }
    }
}
