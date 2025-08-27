using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Messages;

namespace TestUnoAppNavReg.Presentation;

public partial record RightFirstModel
{
    private Entity Entity { get; }
    private INavigator Navigator { get; }
    private IMessenger Messenger { get; }
    private ILogger<LeftFirstModel> Logger { get; }

    public IState<string> Title =>
        State<string>.Value(this, () => $"RightFirst - {Entity?.Name ?? "Empty"}");

    public IListFeed<EntityProperty> Items =>
        ListFeed<EntityProperty>.Async(LoadItemsAsync)
        .Selection(SelectedEntityProperty);

    public IState<EntityProperty> SelectedEntityProperty => State<EntityProperty>.Empty(this)
        .ForEach(action: SelectionChanged);

    public RightFirstModel(
        Entity entity,
        INavigator navigator,
        IMessenger messenger,
        ILogger<LeftFirstModel> logger)
    {
        Entity = entity;
        Navigator = navigator;
        Messenger = messenger;
        Logger = logger;

        // Set default bottom panel 
        Task.Run(() => ShowPropertyDetailAsync(entityProperty: null, ct: default));
    }

    private async ValueTask<IImmutableList<EntityProperty>> LoadItemsAsync(CancellationToken ct)
    {
        if (Entity == null)
            return ImmutableList<EntityProperty>.Empty;

        var items = new List<EntityProperty>() {
            new (Name: $"{Entity.Name} - Property1", TypeName:"First" ),
            new ($"{Entity.Name} - Property2", TypeName:"Second"),
            new ($"{Entity.Name} - Property3", TypeName:"First")
        };

        var firstSelectedEntityProperty = items.FirstOrDefault() ?? EntityProperty.Empty;
        await SelectedEntityProperty.UpdateAsync(old => firstSelectedEntityProperty, ct);

        return await ValueTask.FromResult(items.ToImmutableList());
    }

    private async ValueTask ShowPropertyDetailAsync(EntityProperty? entityProperty, CancellationToken ct)
    {
        Logger.LogInformation("RightFirstModel.ShowPropertyDetailAsync");

        if (entityProperty == null)
            return;

        if (entityProperty.TypeName == "Second") {
            _ = await Navigator.NavigateViewModelAsync<RightFirstSecondModel>(
               this,
               data: entityProperty,
               qualifier: Qualifiers.Nested,
               cancellation: ct);
        } else {
            _ = await Navigator.NavigateViewModelAsync<RightFirstFirstModel>(
                this,
                data: entityProperty,
                qualifier: Qualifiers.Nested,
                cancellation: ct);
        }
    }

    public async ValueTask SelectionChanged(EntityProperty? selectedProperty, CancellationToken ct)
    {
        Logger.LogInformation("RightFirstModel.SelectionChanged");

        await ShowPropertyDetailAsync(selectedProperty, ct);
    }
}
