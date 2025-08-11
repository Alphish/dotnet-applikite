using Alphicsh.Applikite.Models;
using Alphicsh.Applikite.ViewModels.Properties;
using Shouldly;

namespace Alphicsh.Applikite.ViewModels.Tests.Properties;

public class RelayViewModelPropertyTests
{
    [Fact]
    public void PropertyValueSet_ShouldReportValueChange()
    {
        GivenSourceWithInitialValue(123);
        GivenPropertyWithSource("MyProperty");
        GivenNewValue(456);

        WhenPropertyValueSet();

        ThenPropertyValueShouldBe(456);
        ThenSourceChangeShouldBeReported(oldValue: 123, newValue: 456);
        ThenPropertyChangeShouldBeReported(propertyName: "MyProperty", oldValue: 123, newValue: 456);
    }

    [Fact]
    public void SourceValueSet_ShouldReportValueChange()
    {
        GivenSourceWithInitialValue(123);
        GivenPropertyWithSource("MyProperty");
        GivenNewValue(456);

        WhenSourceValueSet();

        ThenPropertyValueShouldBe(456);
        ThenSourceChangeShouldBeReported(oldValue: 123, newValue: 456);
        ThenPropertyChangeShouldBeReported(propertyName: "MyProperty", oldValue: 123, newValue: 456);
    }

    [Fact]
    public void SourceValueSet_ShouldNotReportAfterPropertyDisposal()
    {
        GivenSourceWithInitialValue(123);
        GivenPropertyWithSource("MyProperty");
        GivenNewValue(456);

        WhenPropertyDisposed();
        WhenSourceValueSet();

        ThenPropertyValueShouldBe(456);
        ThenSourceChangeShouldBeReported(oldValue: 123, newValue: 456);
        ThenNoPropertyChangeShouldBeReported();
    }

    [Fact]
    public void PropertyValueSet_ShouldNotReportSettingSameValue()
    {
        GivenSourceWithInitialValue(123);
        GivenPropertyWithSource("MyProperty");
        GivenNewValue(123);

        WhenPropertyValueSet();

        ThenPropertyValueShouldBe(123);
        ThenNoSourceChangeShouldBeReported();
        ThenNoPropertyChangeShouldBeReported();
    }

    // -----
    // Setup
    // -----

    private TestViewModel ViewModel { get; } = new TestViewModel();
    private ValueSource<int> ValueSource { get; set; } = default!;
    private RelayViewModelProperty<int> Property { get; set; } = default!;
    private int NewValue { get; set; }

    private object? ReportedSourceSender { get; set; }
    private ValueChangedEventArgs<int>? ReportedSourceChange { get; set; }
    private object? ReportedPropertySender { get; set; }
    private ValueChangedEventArgs<int>? ReportedPropertyChange { get; set; }

    // Given

    private void GivenSourceWithInitialValue(int value)
    {
        ValueSource = Models.ValueSource.Create(value);
        ValueSource.ValueChanged += (sender, e) =>
        {
            ReportedSourceSender = sender;
            ReportedSourceChange = e;
        };
    }

    private void GivenPropertyWithSource(string propertyName)
    {
        Property = new RelayViewModelProperty<int>(ViewModel, propertyName, ValueSource);
        Property.ValueChanged += (sender, e) =>
        {
            ReportedPropertySender = sender;
            ReportedPropertyChange = e;
        };
    }

    private void GivenNewValue(int value)
        => NewValue = value;

    // When

    private void WhenPropertyValueSet()
        => Property.Value = NewValue;

    private void WhenPropertyDisposed()
        => Property.Dispose();

    private void WhenSourceValueSet()
        => ValueSource.Value = NewValue;

    // Then

    private void ThenPropertyValueShouldBe(int value)
        => Property.Value.ShouldBe(value);

    private void ThenSourceChangeShouldBeReported(int oldValue, int newValue)
    {
        ReportedSourceSender.ShouldBe(ValueSource);
        ReportedSourceChange.ShouldNotBeNull();
        ReportedSourceChange.OldValue.ShouldBe(oldValue);
        ReportedSourceChange.NewValue.ShouldBe(newValue);
    }

    private void ThenNoSourceChangeShouldBeReported()
    {
        ReportedSourceSender.ShouldBeNull();
        ReportedSourceChange.ShouldBeNull();
    }

    private void ThenPropertyChangeShouldBeReported(string propertyName, int oldValue, int newValue)
    {
        ReportedPropertySender.ShouldBe(Property);
        ReportedPropertyChange.ShouldNotBeNull();
        ReportedPropertyChange.OldValue.ShouldBe(oldValue);
        ReportedPropertyChange.NewValue.ShouldBe(newValue);
        ViewModel.ReceivedProperties.ShouldHaveSingleItem();
        ViewModel.ReceivedProperties.ShouldContain(propertyName);
    }

    private void ThenNoPropertyChangeShouldBeReported()
    {
        ReportedPropertySender.ShouldBeNull();
        ReportedPropertyChange.ShouldBeNull();
        ViewModel.ReceivedProperties.ShouldBeEmpty();
    }
}
