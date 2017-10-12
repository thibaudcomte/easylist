using EasyList.Proto.Core;
using EasyList.Proto.Core.Recipes;
using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using EasyList.Proto.DataModels;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace EasyList.Proto
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }

        protected async override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await base.OnInitializeAsync(args);

            await Container.Resolve<RecipesFacade>().InitializeAsync();
            await Container.Resolve<RetailersFacade>().InitializeAsync();
        }

        protected async override Task OnSuspendingApplicationAsync()
        {
            await base.OnSuspendingApplicationAsync();

            await Container.Resolve<RecipesFacade>().UnInitializeAsync();
            await Container.Resolve<RetailersFacade>().UnInitializeAsync();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IRecipesProvider, RecipesJsonProvider>();
            Container.RegisterType<RecipesFacade>(new ContainerControlledLifetimeManager());

            RetailersProvider retailersProvider = new RetailersProvider();
            retailersProvider.Retailers.Add(Container.Resolve<Core.Retailers.Intermarche.Retailer>());

            Container.RegisterInstance<IRetailersProvider>(retailersProvider);
            Container.RegisterType<RetailersFacade>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ShoppingFacade>(new ContainerControlledLifetimeManager());
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.Recipes.ToString(), null);
            return Task.FromResult(true);
        }
    }
}
