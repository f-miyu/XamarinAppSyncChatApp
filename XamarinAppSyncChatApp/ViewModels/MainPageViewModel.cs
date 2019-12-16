using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using XamarinAppSyncChatApp.Models;
using XamarinAppSyncChatApp.Servuces;

namespace XamarinAppSyncChatApp.ViewModels
{
    public class MainPageViewModel : BindableBase, IDestructible
    {
        private readonly IChatClient _chatClient;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public string UserId { get; } = Guid.NewGuid().ToString();

        public ReactivePropertySlim<string> Text { get; } = new ReactivePropertySlim<string>();
        public ReactiveCollection<Message> Messages { get; } = new ReactiveCollection<Message>();
        public AsyncReactiveCommand SendMessageCommand { get; }

        public MainPageViewModel(IChatClient chatClient)
        {
            _chatClient = chatClient;

            SendMessageCommand = Text.Select(s => !string.IsNullOrEmpty(s)).ToAsyncReactiveCommand();
            SendMessageCommand.Subscribe(async () =>
            {
                var text = Text.Value;
                Text.Value = null;
                await _chatClient.CreateMessageAsync("0", text, UserId);
            });

            _chatClient.ObserveMessageCreated("0")
                .Subscribe((message) => Messages.Add(message))
                .AddTo(_disposable);
        }

        public void Destroy()
        {
            _disposable.Dispose();
        }
    }
}
