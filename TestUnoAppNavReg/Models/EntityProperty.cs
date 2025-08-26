namespace TestUnoAppNavReg.Models;

public record EntityProperty(string Name, string TypeName)
{
    public string NameForTitle => $"{Name} ({TypeName})";

    public static EntityProperty Empty => 
        new(Name: "EmptyProperty", TypeName: "Empty");
}
