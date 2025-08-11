using System;

namespace Alphicsh.Applikite.Models;

public class ValueChangedEventArgs<TValue> : EventArgs
{
    public TValue OldValue { get; }
    public TValue NewValue { get; }

    public ValueChangedEventArgs(TValue oldValue, TValue newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}

public static class ValueChangedEventArgs
{
    public static ValueChangedEventArgs<TValue> Create<TValue>(TValue oldValue, TValue newValue)
        => new ValueChangedEventArgs<TValue>(oldValue, newValue);
}
