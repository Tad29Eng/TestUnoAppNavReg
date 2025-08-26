namespace TestUnoAppNavReg.Models;

public record Entity(string Name)
{
    public static Entity Empty => 
        new(Name: "Empty");    
}
