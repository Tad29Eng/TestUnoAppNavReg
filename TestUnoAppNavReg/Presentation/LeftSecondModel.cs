namespace TestUnoAppNavReg.Presentation;

public partial record LeftSecondModel(DataPanel Entity)
{
    public IState<string> Title => 
        State<string>.Value(this, () => $"LeftSecond - {Entity.Name}");
}
