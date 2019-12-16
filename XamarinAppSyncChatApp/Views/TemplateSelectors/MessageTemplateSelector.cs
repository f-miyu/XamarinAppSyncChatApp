using System;
using Xamarin.Forms;
using XamarinAppSyncChatApp.Models;
using XamarinAppSyncChatApp.ViewModels;

namespace XamarinAppSyncChatApp.Views.TemplateSelectors
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessageTemplate { get; set; }
        public DataTemplate OtherMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is Message message) || !(container.BindingContext is MainPageViewModel mainPageViewModel))
                return null;

            return message.UserId == mainPageViewModel.UserId ? MyMessageTemplate : OtherMessageTemplate;
        }
    }
}
