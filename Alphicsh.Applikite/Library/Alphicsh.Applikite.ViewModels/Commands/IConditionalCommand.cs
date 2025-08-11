using System.Windows.Input;

namespace Alphicsh.Applikite.ViewModels.Commands;

public interface IConditionalCommand : ICommand
{
    void RaiseCanExecuteChanged();
}
