using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LandLord.BlazorApp.Shared.General.Toasts
{
    public interface IToastService
    {
        IList<Toast> Toasts { get; }
        void CreateNewToast(Toast toast);
    }
    public enum ToastKind {Info, Success, Warning, Danger, }
    public class Toast 
    {
        public ToastKind Kind { get; set; } = ToastKind.Success;
        //public bool Hidden { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// ms
        /// </summary>
        public int Duration { get; set; } = 5000;
    }

    public class ToastContainer : ComponentBase, IToastService
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        public IList<Toast> Toasts { get; private set; } = new List<Toast>();

        public void CreateNewToast(Toast toast) 
        {
            this.Toasts.Add(toast);
            this.StateHasChanged();
            Console.WriteLine("toast added: Count="+ this.Toasts.Count);
            Timer timer = new Timer(
                (obj) => {
                    this.Toasts.Remove(toast);
                    this.StateHasChanged();
                },
                null,
                toast.Duration,
                Timeout.Infinite
            );
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
                builder.AddAttribute(1, "style", "position: relative; min-height: 200px;");
                builder.OpenComponent<CascadingValue<ToastContainer>>(2);
                    builder.AddAttribute(3, "Value", this);
                    builder.AddAttribute(4, "ChildContent", this.ChildContent);
                builder.CloseComponent();
                builder.OpenElement(5, "div");
                    builder.AddAttribute(6, "style", "position: absolute; top: 0; right: 0;");
                    var index = 7;
                    for (var i = 0; i < this.Toasts.Count; i++)
                    {
                        var item = this.Toasts[i];
                        ////EventCallback cb = EventCallback.Factory.Create(this, ()=> {
                        ////    this.Toasts.Remove(item);
                        ////    this.StateHasChanged();
                        ////});
                        builder.OpenComponent<ToastItem>(index++);
                        builder.AddAttribute(index++, "Kind", item.Kind);
                        builder.AddAttribute(index++, "Title", item.Title);
                        builder.AddAttribute(index++, "Content", item.Content);
                        builder.AddAttribute(index++, "Hidden", false);
                        //builder.AddAttribute(index++, "OnClose", cb);
                        builder.CloseComponent();
                    }
            builder.CloseElement();
            builder.CloseElement();
        }
    }

}
