using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class BottomViewModel : BaseViewModel
{
    public BottomViewModel(AppModel model)
    {
        BottomTextProperty = RelayProperty(nameof(BottomText), model.TextSource);
    }

    public RelayViewModelProperty<string> BottomTextProperty { get; }
    public string BottomText { get => BottomTextProperty.Value; set => BottomTextProperty.Value = value; }
}
