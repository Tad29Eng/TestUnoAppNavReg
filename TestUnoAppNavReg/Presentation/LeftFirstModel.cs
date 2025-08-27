using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Messages;

namespace TestUnoAppNavReg.Presentation;

public partial record LeftFirstModel
{

    private DataPanel Panel { get; }
    
    private IMessenger Messenger { get; }
   
    private ILogger<LeftFirstModel> Logger { get; }

    public IState<string> Title =>
        State<string>.Value(this, () => $"LeftFirst - {Panel?.Name ?? "Empty"}");

    public IListFeed<Entity> Items =>
        ListFeed<Entity>.Async(LoadItemsAsync)
        .Selection(SelectedEntity);

    public IState<Entity> SelectedEntity => State<Entity>.Empty(this)
        .ForEach(action: SelectionChanged);

    public LeftFirstModel(
        DataPanel panel,
        IMessenger messenger,
        ILogger<LeftFirstModel> logger)
    {
        Panel = panel;
        Messenger = messenger;
        Logger = logger;
    }

    private async ValueTask<IImmutableList<Entity>> LoadItemsAsync(CancellationToken ct)
    {
        var items = new List<Entity>() {
            new ("Entity1"),
            new ("Entity2"),
            new ("Entity3")
        };

        return await ValueTask.FromResult(items.ToImmutableList());
    }

    public async ValueTask SelectionChanged(Entity? selectedEntity, CancellationToken ct)
    {
        Logger.LogInformation("LeftFirstModel.SelectionChanged");

        if (selectedEntity == null)
            return;

        Messenger.Send(new LeftFirstShowDetailMessage(selectedEntity));
    }

    public async ValueTask ShowDetailsCommandAsync(CancellationToken ct)
    {
        Logger.LogInformation("LeftFirstModel.ShowDetailsCommandAsync");

        var selectedEntity = await SelectedEntity;
        Messenger.Send(new LeftFirstShowDetailMessage(selectedEntity));
    }
}
