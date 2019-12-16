using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Foundation;
using NativeLibrary;
using XamarinAppSyncChatApp.Servuces;

namespace XamarinAppSyncChatApp.iOS.Services
{
    public class ChatClient : IChatClient
    {
        private readonly AppSyncChatClient _client = new AppSyncChatClient();

        public Task<Models.Message> CreateMessageAsync(string chatId, string text, string userId)
        {
            var tcs = new TaskCompletionSource<Models.Message>();
            _client.CreateMessageWithChatId(chatId, text, userId,
                (message) => tcs.SetResult(message == null ? null : new Models.Message
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    Text = message.Text,
                    UserId = message.UserId
                }),
                (e) => tcs.SetException(new Exception(e.LocalizedDescription)));

            return tcs.Task;
        }

        public IObservable<Models.Message> ObserveMessageCreated(string chatId)
        {
            return Observable.Create<Models.Message>((observer) =>
            {
                NSError error;
                var subscription = _client.OnCreateMessageWithChatId(chatId, out error,
                    (message) => observer.OnNext(message == null ? null : new Models.Message
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        Text = message.Text,
                        UserId = message.UserId
                    }),
                    (e) => observer.OnError(new Exception(e.LocalizedDescription)));

                if (error != null)
                {
                    observer.OnError(new Exception(error.LocalizedDescription));
                }

                return Disposable.Create(() => subscription.Cancel());
            });
        }
    }
}
