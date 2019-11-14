﻿using Blazor.Extensions;
using Itminus.LandLord.BlazorExtensions.SignalR.Patch;
using LandLord.Shared;
using LandLord.Shared.Hub.CallbackArguments;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Services
{
    public class SignalRService
    {
        public HubConnectionEx Connection { get; private set; }
        private ValueTask<object> thenable;

        public SignalRService(IJSRuntime jsRuntime)
        {
            this.Connection = new HubConnectionBuilderEx(jsRuntime)
                .WithUrl("/gamehub")
                .Build();
            Console.WriteLine("Connection:");
            Console.WriteLine(Connection);
            this.Setup();
            //this.PlayCardsCallbackObservable = this.CreateObserverable("PlayCardsCallback");
            //this.PassCardsCallbackObservable = this.CreateObserverable("PassCardsCallback");
            //this.BeLandLordCallbackObservable = this.CreateObserverable("BeLandLordCallback");
            this.StartAsync();
        }

        private void Setup()
        {
            this.Connection.On<string>("ReceiveError", (error) =>
            {
                Console.WriteLine("ReceiveError" + error);
                return Task.CompletedTask;
            });

            this.Connection.On<Guid>("AddingToRoomSucceeded", roomId =>
            {
                Console.WriteLine("AddingToRoomSucceeded" + roomId);
                return Task.CompletedTask;
            });

            this.Connection.On<int>("Win", (index) =>
            {
                Console.WriteLine("Win " + index);
                return Task.CompletedTask;
            });
        }

        private async Task StartAsync()
        {
            this.thenable = this.Connection.StartAsync();
            await this.thenable;
            Console.WriteLine("Connection started");
        }

        public void BindObserver(string cbname, IObserver<CallbackArgs> observer)
        {
            this.Connection.On<CallbackArgs>(cbname, (CallbackArgs cbarg) =>
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
        }



        #region InvokeServerSideMethod


        public async Task PullLatestStateAsync(Guid roomId)
        {
            await this.thenable;
            await this.Connection.InvokeAsync("PushLatestStateToCurrentPlayer", roomId);
        }

        public async Task JoinRoomAsync(Guid roomId)
        {
            await this.thenable;
            await this.Connection.InvokeAsync("AddToRoom", roomId);
        }

        public async Task BeLandLord(Guid roomId)
        {
            await this.thenable;
            await this.Connection.InvokeAsync("BeLandLord", roomId);
        }

        public async Task PlayCards(Guid roomId, object cards)
        {
            await this.thenable;
            await this.Connection.InvokeAsync("PlayCards", roomId, cards);
        }

        public async Task PassCards(Guid roomId)
        {
            await this.thenable;
            await this.Connection.InvokeAsync("PassCards", roomId);
        }
        #endregion InvokeServerSideMethod

    }


}