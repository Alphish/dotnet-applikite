using System;
using System.Windows.Input;

namespace Alphicsh.Applikite.ViewModels.Commands;

public class PlainCommand : ICommand
{
    private Action ExecutionAction { get; }

    // --------
    // Creation
    // --------

    public PlainCommand(Action executionAction)
    {
        ExecutionAction = executionAction;
    }

    // -----------------------
    // ICommand implementation
    // -----------------------

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        ExecutionAction();
    }
}
