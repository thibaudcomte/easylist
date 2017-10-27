using EasyList.Proto.Core.Recipes;
using EasyList.Proto.Core.Retailers;
using EasyList.Proto.Core.Shopping;
using EasyList.Proto.Core.Uwp.Storage.LocalStorage;
using EasyList.Proto.DataModels;
using EasyList.Proto.Retailers.Intermarche;
using EasyList.Proto.Retailers.Intermarche.Uwp;
using EasyList.Proto.Retailers.Carrefour;
using EasyList.Proto.Retailers.Carrefour.Uwp;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
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

            // check out https://mobile.azure.com/users/thibaud.comte/apps/EasyList/analytics/overview
            MobileCenter.Start("fb67d238-7123-4595-8aca-24df688ef958", typeof(Analytics));
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

            RetailersProvider retailersProvider = new RetailersProvider();
            retailersProvider.Retailers.Add(new Retailers.Intermarche.Retailer(new Retailers.Intermarche.Uwp.RetailerSettings()));
            retailersProvider.Retailers.Add(new Retailers.Carrefour.Retailer(new Retailers.Carrefour.Uwp.RetailerSettings()));
            Container.RegisterInstance<IRetailersProvider>(retailersProvider);

            Container.RegisterType<RetailersFacade>(new ContainerControlledLifetimeManager());

            RecipesFacade recipesFacade = new RecipesFacade(new RecipesJsonProvider(), new LocalStorageReaderWriter("favoriteRecipes"));
            Container.RegisterInstance(recipesFacade);

            Container.RegisterType<ShoppingFacade>(new ContainerControlledLifetimeManager());
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.Recipes.ToString(), null);
            return Task.FromResult(true);
        }
    }
}
