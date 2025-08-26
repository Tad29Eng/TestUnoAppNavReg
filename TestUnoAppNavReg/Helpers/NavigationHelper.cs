namespace TestUnoAppNavReg.Helpers;

internal static class NavigationHelper
{
    public const string MainRouteName = "Main";

    public const string LeftRegionName = "LeftRegion";
    public const string RightRegionName = "RightRegion";
    public const string RightFirstBottomRegionName = "RightFirstBottomRegion";

    public const string LeftFirstRouteName = "LeftFirst";
    public static string LeftFirstRoutePath = $"{LeftRegionName}/{LeftFirstRouteName}";

    public const string LeftSecondRouteName = "LeftSecond";
    public static string LeftSecondRoutePath = $"{LeftRegionName}/{LeftSecondRouteName}";

    public const string RightFirstRouteName = "RightFirst";
    public static string RightFirstRoutePath = $"{RightRegionName}/{RightFirstRouteName}";

    public const string RightFirstFirstRouteName = "RightFirstFirst";
    public static string RightFirstFirstRoutePath = $"{RightFirstBottomRegionName}/{RightFirstFirstRouteName}";

    public const string RightFirstSecondRouteName = "RightFirstSecond";
    public static string RightFirstSecondRoutePath = $"{RightFirstBottomRegionName}/{RightFirstSecondRouteName}";

    public const string RightSecondRouteName = "RightSecond";
    public static string RightSecondRoutePath = $"{RightRegionName}/{RightSecondRouteName}";
}
