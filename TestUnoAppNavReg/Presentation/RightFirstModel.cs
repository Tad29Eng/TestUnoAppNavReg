using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Messages;

namespace TestUnoAppNavReg.Presentation;

public partial record RightFirstModel(
    Entity Entity,
    IMessenger Messenger,
    ILogger<LeftFirstModel> Logger)
{
    public IState<string> Title =>
        State<string>.Value(this, () => $"RightFirst - {Entity?.Name ?? "Empty"}");

    public IListFeed<EntityProperty> Items =>
        ListFeed<EntityProperty>.Async(LoadItemsAsync)
        .Selection(SelectedEntity);

    public IState<EntityProperty> SelectedEntity => State<EntityProperty>.Empty(this)
        .ForEach(action: SelectionChanged);

    private async ValueTask<IImmutableList<EntityProperty>> LoadItemsAsync(CancellationToken ct)
    {
        if (Entity == null)
            return ImmutableList<EntityProperty>.Empty;

        var items = new List<EntityProperty>() {
            new ($"{Entity.Name} - Property1"),
            new ($"{Entity.Name} - Property2"),
            new ($"{Entity.Name} - Property3")
        };

        return await ValueTask.FromResult(items.ToImmutableList());
    }

    public async ValueTask SelectionChanged(EntityProperty? selectedProperty, CancellationToken ct)
    {
        Logger.LogInformation("RightFirstModel.SelectionChanged");

        if (selectedProperty == null)
            return;

        Messenger.Send(new RightFirstShowDetailMessage(selectedProperty));
    }


}
