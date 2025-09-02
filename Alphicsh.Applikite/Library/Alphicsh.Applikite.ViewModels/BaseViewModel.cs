using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alphicsh.Applikite.Models;
using Alphicsh.Applikite.ViewModels.Collections;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.ViewModels;

public class BaseViewModel : IViewModel
{
    protected ICollection<IDisposable> OwnDisposables { get; } = new List<IDisposable>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected RelayViewModelProperty<TValue> RelayProperty<TValue>(string propertyName, IValueSource<TValue> valueSource)
        => RegisterDisposable(new RelayViewModelProperty<TValue>(this, propertyName, valueSource));

    protected ICollectionViewModel<TModel, TViewModel> RelayCollection<TModel, TViewModel>(ICollectionSource<TModel> model)
        where TModel : class
        where TViewModel : class, IItemViewModel<TModel, TViewModel>
    {
        return RegisterDisposable(new CollectionViewModel<TModel, TViewModel>(model));
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // -----------
    // Disposables
    // -----------

    protected TDisposable RegisterDisposable<TDisposable>(TDisposable property)
        where TDisposable : IDisposable
    {
        OwnDisposables.Add(property);
        return property;
    }

    public void Dispose()
    {
        foreach (var property in OwnDisposables)
            property.Dispose();
    }
}

public class BaseViewModel<TModel> : BaseViewModel, IViewModel<TModel>
    where TModel : class
{
    public TModel Model { get; }

    public BaseViewModel(TModel model)
    {
        Model = model;
    }
}
