using System;
using System.ComponentModel;

namespace Alphicsh.Applikite.ViewModels;

public interface IViewModel : INotifyPropertyChanged, IDisposable
{
    void RaisePropertyChanged(string propertyName);
}

public interface IViewModel<TModel> : IViewModel
    where TModel : class
{
    TModel Model { get; }
}
