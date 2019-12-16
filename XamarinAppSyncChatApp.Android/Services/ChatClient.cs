using System;
using System.Threading.Tasks;
using Com.Example.Appsyncchatlibrary;
using Java.Lang;
using XamarinAppSyncChatApp.Models;
using XamarinAppSyncChatApp.Servuces;
using Android.Runtime;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using Android.Content;

namespace XamarinAppSyncChatApp.Droid.Services
{
    public class ChatClient : IChatClient
    {
        private readonly AppSyncChatClient _client = new AppSyncChatClient(Android.App.Application.Context);

        public Task<Models.Message> CreateMessageAsync(string chatId, string text, string userId)
        {
            var tcs = new TaskCompletionSource<Models.Message>();
            _client.CreateMessage(chatId, text, userId, new Callback<Com.Example.Appsyncchatlibrary.Message>(
                (message) => tcs.SetResult(message == null ? null : new Models.Message
                {
                    Id = message.Id,
                    ChatId = message.ChatId,
                    Text = message.Text,
                    UserId = message.UserId
                }),
                (e) => tcs.SetException(new System.Exception(e.Message))));

            return tcs.Task;
        }

        public IObservable<Models.Message> ObserveMessageCreated(string chatId)
        {
            return Observable.Create<Models.Message>((observer) =>
            {
                var subscription = _client.OnCreateMessage(chatId, new SubscriptionCallback<Com.Example.Appsyncchatlibrary.Message>(
                    (message) => observer.OnNext(message == null ? null : new Models.Message
                    {
                        Id = message.Id,
                        ChatId = message.ChatId,
                        Text = message.Text,
                        UserId = message.UserId
                    }),
                    (e) => observer.OnError(new System.Exception(e.Message)),
                    () => observer.OnCompleted()));

                return Disposable.Create(() => subscription.Cancel());
            });
        }

        private class Callback<T> : Java.Lang.Object, ICallback where T : Java.Lang.Object
        {
            private readonly Action<T> _onResponse;
            private readonly Action<Java.Lang.Exception> _onFailuer;

            public Callback(Action<T> onResponse, Action<Java.Lang.Exception> onFailuer)
            {
                _onResponse = onResponse;
                _onFailuer = onFailuer;
            }

            public void OnResponse(Java.Lang.Object p0)
            {
                _onResponse?.Invoke(p0.JavaCast<T>());
            }

            public void OnFailure(Java.Lang.Exception p0)
            {
                _onFailuer?.Invoke(p0);
            }
        }

        private class SubscriptionCallback<T> : Java.Lang.Object, ISubscriptionCallback where T : Java.Lang.Object
        {
            private readonly Action<T> _onResponse;
            private readonly Action<Java.Lang.Exception> _onFailure;
            private readonly Action _onComplete;

            public SubscriptionCallback(Action<T> onResponse, Action<Throwable> onFailure, Action onComplete)
            {
                _onResponse = onResponse;
                _onFailure = onFailure;
                _onComplete = onComplete;
            }

            public void OnResponse(Java.Lang.Object p0)
            {
                _onResponse?.Invoke(p0.JavaCast<T>());
            }

            public void OnFailure(Java.Lang.Exception p0)
            {
                _onFailure?.Invoke(p0);
            }

            public void OnCompleted()
            {
                _onComplete?.Invoke();
            }
        }
    }
}
