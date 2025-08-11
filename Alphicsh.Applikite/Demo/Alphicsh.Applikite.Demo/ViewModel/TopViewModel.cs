using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Commands;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class TopViewModel : BaseViewModel
{
    public TopViewModel(AppModel model)
    {
        TopTextProperty = RelayProperty(nameof(TopText), model.TextSource);
        UppercaseTextCommand = Command.From(model.IsUppercaseApplicable, model.UppercaseText);

        model.TextSource.ValueChanged += (sender, e) => UppercaseTextCommand.RaiseCanExecuteChanged();
    }

    public RelayViewModelProperty<string> TopTextProperty { get; }
    public string TopText { get => TopTextProperty.Value; set => TopTextProperty.Value = value; }

    public IConditionalCommand UppercaseTextCommand { get; }
}
