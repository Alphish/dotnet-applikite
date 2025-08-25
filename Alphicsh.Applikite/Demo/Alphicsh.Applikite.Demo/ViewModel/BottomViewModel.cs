using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.Tasks.Progress;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Properties;
using Alphicsh.Applikite.ViewModels.Tasks;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class BottomViewModel : BaseViewModel
{
    public BottomViewModel(AppModel model)
    {
        BottomTextProperty = RelayProperty(nameof(BottomText), model.TextSource);
        GenerateHashTask = new TaskStreamViewModel<string, IntegerProgress>(model.GenerateHashTaskStream);
    }

    public RelayViewModelProperty<string> BottomTextProperty { get; }
    public string BottomText { get => BottomTextProperty.Value; set => BottomTextProperty.Value = value; }

    public TaskStreamViewModel<string, IntegerProgress> GenerateHashTask { get; }
}
