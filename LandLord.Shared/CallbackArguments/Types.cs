using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandLord.Shared.Hub.CallbackArguments
{
    public static class KindValues
    {
        public static string Success = "success";
        public static string Fail = "fail";
    }

    public class CallbackArgs
    {
        public virtual string Kind { get; set; }
    }

    public class BeLandLordCallbackArgs : CallbackArgs
    {
        public int LandLordIndex { get; set; }
    }


    public class PassCardsCallbackArgs : CallbackArgs
    {
        public int Index { get; set; }
    }
}
