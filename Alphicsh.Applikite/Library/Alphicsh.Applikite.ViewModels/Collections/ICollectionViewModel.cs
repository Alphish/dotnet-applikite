using System;
using System.Collections;
using System.Collections.Specialized;
using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Collections;

public interface ICollectionViewModel<TModel, TViewModel> : INotifyCollectionChanged, IList, IDisposable
    where TViewModel : IViewModel
{
    ICollectionSource<TModel> Model { get; }
}
