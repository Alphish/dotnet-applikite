using System;

namespace Alphicsh.Applikite.Models;

public interface IValueSource<TValue>
{
    TValue Value { get; set; }
    event EventHandler<ValueChangedEventArgs<TValue>>? ValueChanged;
}
