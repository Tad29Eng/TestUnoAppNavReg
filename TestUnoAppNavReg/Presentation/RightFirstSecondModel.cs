namespace TestUnoAppNavReg.Presentation;

public partial record RightFirstSecondModel(EntityProperty Property)
{
    public IState<string> Title =>
        State<string>.Value(this, () => $"RightFirstSecond - {Property.NameForTitle}");
}
