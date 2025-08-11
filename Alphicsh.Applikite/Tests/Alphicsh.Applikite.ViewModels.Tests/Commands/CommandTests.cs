using Alphicsh.Applikite.ViewModels.Commands;
using Shouldly;

namespace Alphicsh.Applikite.ViewModels.Tests.Commands;

public class CommandTests
{
    // no condition, no parameter

    [Fact]
    public void UnconditionalParameterlessCommand_ShouldAllowExecution()
    {
        var command = Command.From(SetLorem);
        command.CanExecute(parameter: null).ShouldBeTrue();
    }

    [Fact]
    public void UnconditionalParameterlessCommand_ShouldExecute()
    {
        var command = Command.From(SetLorem);
        command.Execute(parameter: null);
        ReceivedValue.ShouldBe("lorem");
    }

    // no condition, optional parameter

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldAllowExecutionOfNull()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        command.CanExecute(parameter: null).ShouldBeTrue();
    }

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldExecuteNull()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        command.Execute(parameter: null);
        ReceivedValue.ShouldBeNull();
    }

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldAllowExecutionOfString()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        command.CanExecute(parameter: "whatever").ShouldBeTrue();
    }

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldExecuteString()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        command.Execute(parameter: "whatever");
        ReceivedValue.ShouldBe("whatever");
    }

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldDisallowExecutionOfInt()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        command.CanExecute(parameter: 123).ShouldBeFalse();
    }

    [Fact]
    public void UnconditionalOptionalArgumentCommand_ShouldThrowExecutingInt()
    {
        var command = Command.WithOptionalParameter<string>(SetWord);
        Action executionAttempt = () => command.Execute(parameter: 123);
        executionAttempt.ShouldThrow<ArgumentException>();
    }

    // no condition, required parameter

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldDisallowExecutionOfNull()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        command.CanExecute(parameter: null).ShouldBeFalse();
    }

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldThrowExecutingNull()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        Action executionAttempt = () => command.Execute(parameter: null);
        executionAttempt.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldAllowExecutionOfString()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        command.CanExecute(parameter: "whatever").ShouldBeTrue();
    }

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldExecuteString()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        command.Execute(parameter: "whatever");
        ReceivedValue.ShouldBe("whatever");
    }

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldDisallowExecutionOfInt()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        command.CanExecute(parameter: 123).ShouldBeFalse();
    }

    [Fact]
    public void UnconditionalRequiredArgumentCommand_ShouldThrowExecutingInt()
    {
        var command = Command.WithRequiredParameter<string>(SetWord);
        Action executionAttempt = () => command.Execute(parameter: 123);
        executionAttempt.ShouldThrow<ArgumentException>();
    }

    // conditional, no parameter

    [Fact]
    public void ConditionalParameterlessCommand_ShouldAllowExecutionWhenConditionMet()
    {
        AllowExecution = true;
        var command = Command.From(IsExecutionAllowed, SetLorem);
        command.CanExecute(parameter: null).ShouldBeTrue();
    }

    [Fact]
    public void ConditionalParameterlessCommand_ShouldDisallowExecutionWhenConditionUnmet()
    {
        AllowExecution = false;
        var command = Command.From(IsExecutionAllowed, SetLorem);
        command.CanExecute(parameter: null).ShouldBeFalse();
    }

    [Fact]
    public void ConditionalParameterlessCommand_ShouldExecute()
    {
        var command = Command.From(IsExecutionAllowed, SetLorem);
        command.Execute(parameter: null);
        ReceivedValue.ShouldBe("lorem");
    }

    [Fact]
    public void ConditionalParameterlessCommand_ShouldRaiseCanExecuteChangedEvent()
    {
        object? receivedSender = null;
        EventArgs? receivedArgs = null;

        AllowExecution = true;
        var command = Command.From(IsExecutionAllowed, SetLorem);
        command.CanExecuteChanged += (sender, e) =>
        {
            receivedSender = sender;
            receivedArgs = e;
        };

        AllowExecution = false;
        command.RaiseCanExecuteChanged();

        receivedSender.ShouldBe(command);
        receivedArgs.ShouldNotBeNull();
    }

    // conditional, optional parameter

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldAllowExecutionOfNull()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: null).ShouldBeTrue();
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldExecuteNull()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.Execute(parameter: null);
        ReceivedValue.ShouldBeNull();
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldAllowExecutionOfValidString()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: "whatever").ShouldBeTrue();
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldDisallowExecutionOfInvalidString()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: "curse").ShouldBeFalse();
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldExecuteValidString()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.Execute(parameter: "whatever");
        ReceivedValue.ShouldBe("whatever");
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldDisallowExecutionOfInt()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: 123).ShouldBeFalse();
    }

    [Fact]
    public void ConditionalOptionalArgumentCommand_ShouldThrowExecutingInt()
    {
        var command = Command.WithOptionalParameter<string>(IsNotCurse, SetWord);
        Action executionAttempt = () => command.Execute(parameter: 123);
        executionAttempt.ShouldThrow<ArgumentException>();
    }

    // conditional, required parameter

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldDisallowExecutionOfNull()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: null).ShouldBeFalse();
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldThrowExecutingNull()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        Action executionAttempt = () => command.Execute(parameter: null);
        executionAttempt.ShouldThrow<ArgumentNullException>();
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldAllowExecutionOfValidString()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: "whatever").ShouldBeTrue();
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldDisallowExecutionOfInvalidString()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: "curse").ShouldBeFalse();
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldExecuteString()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        command.Execute(parameter: "whatever");
        ReceivedValue.ShouldBe("whatever");
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldDisallowExecutionOfInt()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        command.CanExecute(parameter: 123).ShouldBeFalse();
    }

    [Fact]
    public void ConditionalRequiredArgumentCommand_ShouldThrowExecutingInt()
    {
        var command = Command.WithRequiredParameter<string>(IsNotCurse, SetWord);
        Action executionAttempt = () => command.Execute(parameter: 123);
        executionAttempt.ShouldThrow<ArgumentException>();
    }

    // -----
    // Setup
    // -----

    private bool AllowExecution { get; set; }
    private string? ReceivedValue { get; set; }

    private void SetLorem()
        => ReceivedValue = "lorem";

    private void SetWord(string word)
        => ReceivedValue = word;

    private bool IsExecutionAllowed()
        => AllowExecution;
    private bool IsNotCurse(string word)
        => word != "curse";
}
