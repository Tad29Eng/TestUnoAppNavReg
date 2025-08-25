using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Helpers;
using Uno.Resizetizer;

namespace TestUnoAppNavReg;
public partial class App : Application
{
    /// <summary>
    /// Initializes the singleton application object. This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }
    protected IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                // Switch to Development environment when running in DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) => {
                    // Configure log levels for different categories of logging
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)

                        // Default filters for core Uno Platform namespaces
                        .CoreLogLevel(LogLevel.Warning);

                    // Uno Platform namespace filter groups
                    // Uncomment individual methods to see more detailed logging
                    //// Generic Xaml events
                    //logBuilder.XamlLogLevel(LogLevel.Debug);
                    //// Layout specific messages
                    //logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
                    //// Storage messages
                    //logBuilder.StorageLogLevel(LogLevel.Debug);
                    //// Binding related messages
                    //logBuilder.XamlBindingLogLevel(LogLevel.Debug);
                    //// Binder memory references tracking
                    //logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
                    //// DevServer and HotReload related
                    //logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
                    //// Debug JS interop
                    //logBuilder.WebAssemblyLogLevel(LogLevel.Debug);

                }, enableUnoLogging: true)
                .UseSerilog(consoleLoggingEnabled: true, fileLoggingEnabled: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                // Enable localization (see appsettings.json for supported languages)
                .UseLocalization()
                .ConfigureServices((context, services) => {
                    services.AddSingleton<IMessenger, WeakReferenceMessenger>();
                })
                .UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)
            );
        MainWindow = builder.Window;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<MainPage, MainModel>(),
            new DataViewMap<LeftFirstPage, LeftFirstModel, DataPanel>(),
            new DataViewMap<LeftSecondPage, LeftSecondModel, DataPanel>(),
            new DataViewMap<RightFirstPage, RightFirstModel, Entity>(),
            new DataViewMap<RightFirstPage, RightSecondModel, Entity>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested:
                [
                    new (NavigationHelper.MainRouteName, View: views.FindByViewModel<MainModel>(), IsDefault:true,
                    Nested:[
                        new (NavigationHelper.LeftRegionName,
                        Nested: [
                            //new (Helpers.NavigationHelper.LeftFirstRouteName, View: views.FindByViewModel<LeftFirstModel>(), IsDefault:true),
                            //new (Helpers.NavigationHelper.LeftSecondRouteName, View: views.FindByViewModel<LeftSecondModel>())

                            new (NavigationHelper.LeftFirstRoutePath, View: views.FindByViewModel<LeftFirstModel>(), IsDefault:true),
                            new (NavigationHelper.LeftSecondRoutePath, View: views.FindByViewModel<LeftSecondModel>())
                        ]),
                        new (NavigationHelper.RightRegionName,
                        Nested:[
                            //new (Helpers.NavigationHelper.RightFirstRouteName, View: views.FindByViewModel<RightFirstModel>(), IsDefault:true),
                            //new (Helpers.NavigationHelper.RightSecondRouteName, View: views.FindByViewModel<RightSecondModel>())
                            
                            new (NavigationHelper.RightFirstRoutePath, View: views.FindByViewModel<RightFirstModel>(), IsDefault:true),
                            new (NavigationHelper.RightSecondRoutePath, View: views.FindByViewModel<RightSecondModel>())
                        ])
                    ])
                ]
            )
        );
    }
}
