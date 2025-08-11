namespace Alphicsh.Applikite.ViewModels.Commands;

public class PredicateCommand : IConditionalCommand
{
    private Func<object?, bool> ExecutionPredicate { get; }
    private Action<object?> ExecutionAction { get; }

    // --------
    // Creation
    // --------

    public PredicateCommand(Func<bool> executionPredicate, Action executionAction)
    {
        ExecutionPredicate = (object? parameter) => executionPredicate();
        ExecutionAction = (object? parameter) => executionAction();
    }

    public PredicateCommand(Func<object?, bool> executionCondition, Action<object?> executionAction)
    {
        ExecutionPredicate = executionCondition;
        ExecutionAction = executionAction;
    }

    // ----------------------------------
    // IConditionalCommand implementation
    // ----------------------------------

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, new EventArgs());
    }

    public bool CanExecute(object? parameter)
    {
        return ExecutionPredicate(parameter);
    }

    public void Execute(object? parameter)
    {
        ExecutionAction(parameter);
    }
}
