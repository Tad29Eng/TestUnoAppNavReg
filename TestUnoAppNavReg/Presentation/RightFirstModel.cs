using CommunityToolkit.Mvvm.Messaging;
using TestUnoAppNavReg.Messages;

namespace TestUnoAppNavReg.Presentation;

public partial record RightFirstModel(
    Entity Entity,
    INavigator Navigator,
    IMessenger Messenger,
    ILogger<LeftFirstModel> Logger)
{
    public IState<string> Title =>
        State<string>.Value(this, () => $"RightFirst - {Entity?.Name ?? "Empty"}");

    public IListFeed<EntityProperty> Items =>
        ListFeed<EntityProperty>.Async(LoadItemsAsync)
        .Selection(SelectedEntityProperty);

    public IState<EntityProperty> SelectedEntityProperty => State<EntityProperty>.Empty(this)
        .ForEach(action: SelectionChanged);

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

    public async ValueTask SelectionChanged(EntityProperty? selectedProperty, CancellationToken ct)
    {
        Logger.LogInformation("RightFirstModel.SelectionChanged");

        if (selectedProperty == null)
            return;

        if (selectedProperty.TypeName == "Second") {
            _ = await Navigator.NavigateViewModelAsync<RightFirstSecondModel>(
               this,
               data: selectedProperty,
               qualifier: Qualifiers.Nested);
        } else {
            _ = await Navigator.NavigateViewModelAsync<RightFirstFirstModel>(
                this,
                data: selectedProperty,
                qualifier: Qualifiers.Nested);
        }
    }
}
