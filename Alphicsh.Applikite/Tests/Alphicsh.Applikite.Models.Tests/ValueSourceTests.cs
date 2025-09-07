using Alphicsh.Applikite.Models;
using Shouldly;

namespace Alphicsh.Applikite.Files.Tests.Models;

public class ValueSourceTests
{
    [Fact]
    public void ValueSet_ShouldReportValueChange()
    {
        GivenSourceWithInitialValue(123);
        GivenNewValue(456);
        WhenNewValueSet();
        ThenExpectValue(456);
        ThenExpectValueChangeEvent(oldValue: 123, newValue: 456);
    }

    [Fact]
    public void ValueSet_ShouldNotReportSettingSameValue()
    {
        GivenSourceWithInitialValue(123);
        GivenNewValue(123);
        WhenNewValueSet();
        ThenExpectValue(123);
        ThenExpectNoValueChangeEvent();
    }

    // -----
    // Setup
    // -----

    private ValueSource<int> Source { get; set; } = default!;
    private int NewValue { get; set; }
    private object? ReportedSender { get; set; }
    private ValueChangedEventArgs<int>? ReportedChange { get; set; }

    private void GivenSourceWithInitialValue(int value)
    {
        Source = ValueSource.Create(value);
        Source.ValueChanged += (sender, e) =>
        {
            ReportedSender = sender;
            ReportedChange = e;
        };
    }

    private void GivenNewValue(int value)
        => NewValue = value;

    private void WhenNewValueSet()
        => Source.Value = NewValue;


    private void ThenExpectValue(int value)
        => Source.Value.ShouldBe(value);

    private void ThenExpectValueChangeEvent(int oldValue, int newValue)
    {
        ReportedSender.ShouldBe(Source);
        ReportedChange.ShouldNotBeNull();
        ReportedChange.OldValue.ShouldBe(oldValue);
        ReportedChange.NewValue.ShouldBe(newValue);
    }

    private void ThenExpectNoValueChangeEvent()
    {
        ReportedSender.ShouldBeNull();
        ReportedChange.ShouldBeNull();
    }
}
