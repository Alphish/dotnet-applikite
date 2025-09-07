using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Collections;

public interface ICollectionViewModel<TModel, TViewModel> : INotifyCollectionChanged, IList<TViewModel>, IList, IDisposable
    where TViewModel : IViewModel
{
    ICollectionSource<TModel> Model { get; }
    void ReplaceItems(IEnumerable<TViewModel> items);

    // resolving ambiguities between IList<TViewModel> and IList
    // at the moment of writing this comment, Avalonia requires implementing non-generic IList
    // for item sources of ListBox and possibly other components
    new int Count { get; }
    new TViewModel this[int index] { get; set; }
    new void Clear();
    new void RemoveAt(int index);
}
