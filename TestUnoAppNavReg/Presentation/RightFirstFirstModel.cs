namespace TestUnoAppNavReg.Presentation;

public partial record RightFirstFirstModel(EntityProperty Property)
{
    public IState<string> Title =>
        State<string>.Value(this, () => $"RightFirstFirst - {Property.NameForTitle}");
}
