using System.Windows.Input;

namespace Alphicsh.Applikite.ViewModels.Commands;

public static class Command
{
    // --------
    // Commands
    // --------

    public static ICommand From(Action executionAction)
        => new PlainCommand(executionAction);

    public static ICommand WithOptionalParameter<TParameter>(Action<TParameter> executionAction)
    {
        Func<object?, bool> parameterPredicate = parameter => parameter is TParameter || parameter == null;
        return new PredicateCommand(parameterPredicate, ObjectActionOf(executionAction, argumentRequired: false));
    }

    public static IConditionalCommand WithRequiredParameter<TParameter>(Action<TParameter> executionAction)
    {
        Func<object?, bool> parameterPredicate = parameter => parameter is TParameter;
        return new PredicateCommand(parameterPredicate, ObjectActionOf(executionAction, argumentRequired: true));
    }

    public static IConditionalCommand From(Func<bool> executionPredicate, Action executionAction)
        => new PredicateCommand(executionPredicate, executionAction);

    public static IConditionalCommand WithOptionalParameter<TParameter>(
        Func<TParameter, bool> executionPredicate,
        Action<TParameter> executionAction
        )
    {
        return new PredicateCommand(
            ObjectConditionOf(executionPredicate, argumentRequired: false),
            ObjectActionOf(executionAction, argumentRequired: false)
            );
    }

    public static IConditionalCommand WithRequiredParameter<TParameter>(
        Func<TParameter, bool> executionPredicate,
        Action<TParameter> executionAction
        )
    {
        return new PredicateCommand(
            ObjectConditionOf(executionPredicate, argumentRequired: true),
            ObjectActionOf(executionAction, argumentRequired: true)
            );
    }

    // -------
    // Helpers
    // -------

    // converting actions/predicates from generically-typed to object-typed variations
    // because commands accept object-typed variations

    private static Action<object?> ObjectActionOf<TParameter>(Action<TParameter> typedAction, bool argumentRequired)
    {
        return (object? parameter) =>
        {
            if (parameter == null)
            {
                if (argumentRequired)
                    throw new ArgumentNullException($"A parameter cannot be null.");

                typedAction(default!);
                return;
            }

            if (parameter is not TParameter typedParameter)
                throw new ArgumentException($"A parameter of type {typeof(TParameter).Name} is required.");

            typedAction(typedParameter);
        };
    }

    private static Func<object?, bool> ObjectConditionOf<TParameter>(Func<TParameter, bool> typedCondition, bool argumentRequired)
    {
        return (object? parameter) =>
        {
            if (parameter == null)
                return !argumentRequired && typedCondition(default!);

            if (parameter is not TParameter typedParameter)
                return false;

            return typedCondition(typedParameter);
        };
    }
}

