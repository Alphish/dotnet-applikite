using System;
using System.Runtime.CompilerServices;

namespace Alphicsh.Applikite.Tasks;

public interface IAwaitable<TResult>
{
    TaskAwaiter<TResult> GetAwaiter();
    TResult? Result { get; }
    bool RanToCompletion { get; }
    event EventHandler<TResult>? TaskCompleted;
}
