using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alphicsh.Applikite.Models;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.ViewModels;

public class BaseViewModel : IViewModel
{
    protected ICollection<IViewModelProperty> OwnProperties { get; } = new List<IViewModelProperty>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected TProperty RegisterProperty<TProperty>(TProperty property)
        where TProperty : IViewModelProperty
    {
        OwnProperties.Add(property);
        return property;
    }

    protected RelayViewModelProperty<TValue> RelayProperty<TValue>(string propertyName, IValueSource<TValue> valueSource)
        => RegisterProperty(new RelayViewModelProperty<TValue>(this, propertyName, valueSource));

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        foreach (var property in OwnProperties)
            property.Dispose();
    }
}

