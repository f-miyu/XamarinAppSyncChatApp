using System;
using System.Linq;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinAppSyncChatApp
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            var viewTypes = GetType().Assembly.DefinedTypes.Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(Page))).Select(t => t.AsType());
            foreach (var viewType in viewTypes)
            {
                var name = viewType.Name;
                containerRegistry.RegisterForNavigation(viewType, name);
            }
        }
    }
}
