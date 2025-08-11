using System;

namespace Alphicsh.Applikite.Models;

public class ValueSource<TValue> : IValueSource<TValue>
{
    private TValue InnerValue { get; set; }
    public event EventHandler<ValueChangedEventArgs<TValue>>? ValueChanged;

    public ValueSource(TValue value)
    {
        InnerValue = value;
    }

    public TValue Value
    {
        get => InnerValue;
        set
        {
            if (Equals(InnerValue, value))
                return;

            var previousValue = InnerValue;
            InnerValue = value;
            ValueChanged?.Invoke(this, ValueChangedEventArgs.Create(previousValue, InnerValue));
        }
    }
}

public static class ValueSource
{
    public static ValueSource<TValue> Create<TValue>(TValue value)
        => new ValueSource<TValue>(value);
}
