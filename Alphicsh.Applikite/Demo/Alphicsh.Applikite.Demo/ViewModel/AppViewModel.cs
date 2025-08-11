using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class AppViewModel : BaseViewModel
{
    public AppViewModel(AppModel model)
    {
        Top = new TopViewModel(model);
        Bottom = new BottomViewModel(model);
    }

    public TopViewModel Top { get; }
    public BottomViewModel Bottom { get; }
}
