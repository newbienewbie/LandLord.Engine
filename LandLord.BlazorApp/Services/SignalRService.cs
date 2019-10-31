using Blazor.Extensions;
using LandLord.Shared.Hub.CallbackArguments;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Services
{
    public class SignalRService
    {
        private HubConnection connection;
        private ValueTask<object> thenable;

        public IObservable<CallbackArgs> ReceiveStateObservable { get; }
        public IObservable<CallbackArgs> PlayCardsCallbackObservable { get; }
        public IObservable<CallbackArgs> PassCardsCallbackObservable { get; }
        public IObservable<CallbackArgs> BeLandLordCallbackObservable { get; }

        public SignalRService(IJSRuntime jsRuntime)
        {
            this.connection = new HubConnectionBuilder(jsRuntime)
                .WithUrl("/gamehub")
                .Build();

            this.Setup();
            this.ReceiveStateObservable = this.CreateStateObserverable("ReceiveState");
            this.PlayCardsCallbackObservable = this.CreateObserverable("PlayCardsCallback");
            this.PassCardsCallbackObservable = this.CreateObserverable("PassCardsCallback");
            this.BeLandLordCallbackObservable = this.CreateObserverable("BeLandLordCallback");
            this.StartAsync();
        }

        private void Setup()
        {
            this.connection.On<string>("ReceiveError", (error) =>
            {
                Console.WriteLine("ReceiveError" + error);
                return Task.CompletedTask;
            });

            this.connection.On<Guid>("AddingToRoomSucceeded", roomId =>
            {
                Console.WriteLine("AddingToRoomSucceeded" + roomId);
                return Task.CompletedTask;
            });

            this.connection.On<int>("Win", (index) =>
            {
                Console.WriteLine("Win " + index);
                return Task.CompletedTask;
            });
        }

        private async Task StartAsync()
        {
            this.thenable = this.connection.StartAsync();
            await this.thenable;
            Console.WriteLine("Connection started");
        }

        private IObservable<CallbackArgs> CreateStateObserverable(string cbname)
        {
            return Observable.Create<CallbackArgs>(observer =>
            {
                this.connection.On<CallbackArgs>(cbname, (cbarg) =>
                {
                    observer.OnNext(cbarg);
                    return Task.CompletedTask;
                });
                return Task.CompletedTask;
            });
        }

        private IObservable<CallbackArgs> CreateObserverable(string cbname)
        {
            return Observable.Create<CallbackArgs>(observer =>
            {
                this.connection.On<CallbackArgs>(cbname, (CallbackArgs cbarg) =>
                {
                    if (cbarg.Kind == KindValues.Success)
                    {
                        observer.OnNext(cbarg);
                    }
                    else if (cbarg.Kind == KindValues.Fail)
                    {
                        observer.OnError(new Exception(cbarg.ToString()));
                    }
                    return Task.CompletedTask;
                });
                return Task.CompletedTask;
            });
        }



        #region InvokeServerSideMethod


        public async Task PullLatestStateAsync(Guid roomId)
        {
            await this.thenable;
            await this.connection.InvokeAsync("PushLatestStateToCurrentPlayer", roomId);
        }

        public async Task JoinRoomAsync(Guid roomId)
        {
            await this.thenable;
            await this.connection.InvokeAsync("AddToRoom", roomId);
        }

        public async Task BeLandLord(Guid roomId)
        {
            await this.thenable;
            await this.connection.InvokeAsync("BeLandLord", roomId);
        }

        public async Task PlayCards(Guid roomId, object cards)
        {
            await this.thenable;
            await this.connection.InvokeAsync("PlayCards", roomId, cards);
        }

        public async Task PassCards(Guid roomId)
        {
            await this.thenable;
            await this.connection.InvokeAsync("PassCards", roomId);
        }
        #endregion InvokeServerSideMethod

    }


}