using LandLord.BlazorApp.Pages;
using LandLord.Shared;
using LandLord.Shared.Hub.CallbackArguments;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Services
{
    public class SignalRObserver : IObserver<CallbackArgs>
    {
        public virtual void OnCompleted()
        {
            Console.WriteLine("observable completed!");
        }

        public virtual void OnError(Exception error)
        {
            Console.WriteLine("error: " + error.Message);
        }

        public virtual void OnNext(CallbackArgs value) { }
    }

    public class PlayCardsObserver : SignalRObserver
    {

        public override void OnNext(CallbackArgs value)
        {
        }
    }
}