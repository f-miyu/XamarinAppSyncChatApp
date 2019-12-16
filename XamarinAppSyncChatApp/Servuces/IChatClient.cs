using System;
using System.Threading.Tasks;
using XamarinAppSyncChatApp.Models;

namespace XamarinAppSyncChatApp.Servuces
{
    public interface IChatClient
    {
        Task<Message> CreateMessageAsync(string chatId, string text, string userId);
        IObservable<Message> ObserveMessageCreated(string chatId);
    }
}
