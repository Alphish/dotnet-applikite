using System;
using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Properties;

public class RelayViewModelProperty<TValue> : IViewModelProperty<TValue>
{
    public IViewModel ViewModel { get; }
    public string PropertyName { get; }

    private IValueSource<TValue> ValueSource { get; }
    public TValue Value { get => ValueSource.Value; set => ValueSource.Value = value; }

    public event EventHandler<ValueChangedEventArgs<TValue>>? ValueChanged;

    public RelayViewModelProperty(IViewModel viewModel, string propertyName, IValueSource<TValue> valueSource)
    {
        ViewModel = viewModel;
        PropertyName = propertyName;
        ValueSource = valueSource;
        ValueSource.ValueChanged += RelayValueChange;
    }

    public void RaiseValueChanged(TValue oldValue, TValue newValue)
    {
        ViewModel.RaisePropertyChanged(PropertyName);
        ValueChanged?.Invoke(this, ValueChangedEventArgs.Create(oldValue, newValue));
    }

    private void RelayValueChange(object? sender, ValueChangedEventArgs<TValue> valueChange)
        => RaiseValueChanged(valueChange.OldValue, valueChange.NewValue);

    public void Dispose()
    {
        ValueSource.ValueChanged -= RelayValueChange;
    }
}
