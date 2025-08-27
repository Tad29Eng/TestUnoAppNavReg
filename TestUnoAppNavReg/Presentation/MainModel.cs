using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Messages;

namespace TestUnoAppNavReg.Presentation;

public partial record MainModel
{
    private INavigator Navigator { get; }

    private IMessenger Messenger { get; }

    private ILogger<MainModel> Logger { get; }

    public IState<string> Title => State<string>.Value(this, () => "Navigation using reg + cctrl");

    public MainModel(
        INavigator navigator,
        IMessenger messenger,
        ILogger<MainModel> logger)
    {
        Navigator = navigator;
        Messenger = messenger;
        Logger = logger;

        // Set default left panel 
        Task.Run(() => ShowFirstCommandAsync(ct: default));

        // Common Toolkit MVVM Messenger
        Messenger.Register<MainModel, LeftFirstShowDetailMessage>(
            recipient: this,
            handler: async (recipient, message) => await recipient.LeftFirstShowDetailMessageReceived(message));
    }

    private async ValueTask LeftFirstShowDetailMessageReceived(LeftFirstShowDetailMessage message)
    {
        //_ = await Navigator.NavigateRouteAsync(
        //    this,
        //    route: NavigationHelper.RightFirstRoutePath,
        //    data: message.Entity,
        //    qualifier: Qualifiers.Nested);

        _ = await Navigator.NavigateViewModelAsync<RightFirstModel>(
            this,
            data: message.Entity,
            qualifier: Qualifiers.Nested);
    }

    public async ValueTask ShowFirstCommandAsync(CancellationToken ct)
    {
        Logger.LogInformation("Main.ShowFirstCommandAsync");

        //_ = await Navigator.NavigateRouteAsync(
        //    this,
        //    route: NavigationHelper.LeftFirstRoutePath,
        //    data: new DataPanel(Name: "ELFirst"),
        //    qualifier: Qualifiers.Nested,
        //    cancellation: ct);

        // Left
        _ = await Navigator.NavigateViewModelAsync<LeftFirstModel>(
            this,
            data: new DataPanel(Name: "ELFirst"),
            qualifier: Qualifiers.Nested,
            cancellation: ct);

        // Right
        _ = await Navigator.NavigateViewModelAsync<RightFirstModel>(
            this,
            data: null,
            qualifier: Qualifiers.Nested,
            cancellation: ct);
    }

    public async ValueTask ShowSecondCommandAsync(CancellationToken ct)
    {
        Logger.LogInformation("Main.ShowSecondCommandAsync");

        //_ = await Navigator.NavigateRouteAsync(
        //    this, 
        //    route: NavigationHelper.LeftSecondRoutePath,
        //    data: new DataPanel(Name: "ELSecond"),
        //    qualifier: Qualifiers.Nested,
        //    cancellation: ct);

        // Left
        _ = await Navigator.NavigateViewModelAsync<LeftSecondModel>(
            this,
            data: new DataPanel(Name: "ELSecond"),
            qualifier: Qualifiers.Nested,
            cancellation: ct);

        // Right
        _ = await Navigator.NavigateViewModelAsync<RightSecondModel>(
            this,
            data: null,
            qualifier: Qualifiers.Nested,
            cancellation: ct);
    }

}
