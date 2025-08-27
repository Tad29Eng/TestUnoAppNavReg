using TestUnoAppNavReg.Helpers;

namespace TestUnoAppNavReg.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();

        Region.SetName(ConCtrlLeftRegion, NavigationHelper.LeftRegionName);
        Region.SetName(ConCtrlRightRegion, NavigationHelper.RightRegionName);
    }
}
