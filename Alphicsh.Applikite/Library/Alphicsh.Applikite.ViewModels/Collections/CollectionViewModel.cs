using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.ViewModels.Collections;

public class CollectionViewModel<TModel, TViewModel> : ICollectionViewModel<TModel, TViewModel>
    where TModel : class
    where TViewModel : class, IItemViewModel<TModel, TViewModel>
{
    public ICollectionSource<TModel> Model { get; }
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private List<TViewModel> ViewModels { get; set; }

    public CollectionViewModel(ICollectionSource<TModel> model)
    {
        Model = model;
        ViewModels = Model.Select(TViewModel.FromModel).ToList();

        Model.CollectionChanged += ApplyChanges;
    }

    // -----------
    // Enumeration
    // -----------

    public IEnumerator<TViewModel> GetEnumerator()
        => ViewModels.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ViewModels.GetEnumerator();

    // ----------
    // Collection
    // ----------

    public int Count => ViewModels.Count;
    public bool IsReadOnly => false;

    bool ICollection.IsSynchronized => (ViewModels as ICollection).IsSynchronized;
    object ICollection.SyncRoot => (ViewModels as ICollection).SyncRoot;

    public bool Contains(TViewModel item)
        => Model.Contains(item.Model);

    bool IList.Contains(object? value)
        => value is TViewModel itemViewModel && Contains(itemViewModel);

    public void CopyTo(TViewModel[] array, int arrayIndex)
        => ViewModels.CopyTo(array, arrayIndex);

    void ICollection.CopyTo(Array array, int index)
        => (ViewModels as ICollection).CopyTo(array, index);

    public void Add(TViewModel item)
        => Model.Add(item.Model);

    int IList.Add(object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Add(itemViewModel);
        return Model.Count - 1;
    }

    public void Clear()
        => Model.Clear();

    public bool Remove(TViewModel item)
        => Model.Remove(item.Model);

    void IList.Remove(object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Remove(itemViewModel);
    }

    // --------------------
    // IList implementation
    // --------------------

    public TViewModel this[int index] { get => ViewModels[index]; set => Model[index] = value.Model; }
    object? IList.this[int index] { get => this[index]; set => this[index] = (TViewModel)value!; }

    bool IList.IsFixedSize => false;

    public int IndexOf(TViewModel item)
        => Model.IndexOf(item.Model);

    int IList.IndexOf(object? value)
        => value is TViewModel itemViewModel ? IndexOf(itemViewModel) : -1;

    public void Insert(int index, TViewModel item)
        => Model.Insert(index, item.Model);

    void IList.Insert(int index, object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Insert(index, itemViewModel);
    }

    public void RemoveAt(int index)
        => Model.RemoveAt(index);

    // -----------
    // Own methods
    // -----------

    public void ReplaceItems(IEnumerable<TViewModel> items)
    {
        var models = items.Select(item => item.Model);
        Model.ReplaceItems(models);
    }

    // ---------------
    // Synchronization
    // ---------------

    private void ApplyChanges(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var itemsToAdd = e.NewItems!.OfType<TModel>().Select(TViewModel.FromModel).ToList();
                ViewModels.InsertRange(e.NewStartingIndex, itemsToAdd);
                RaiseInsert(e.NewStartingIndex, itemsToAdd);
                break;

            case NotifyCollectionChangedAction.Remove:
                var removeCount = e.OldItems!.Count;
                var removedItems = ViewModels.Skip(e.OldStartingIndex).Take(removeCount).ToList();
                removedItems.ForEach(item => item.Dispose());
                ViewModels.RemoveRange(e.OldStartingIndex, removeCount);
                RaiseRemove(e.OldStartingIndex, removedItems);
                break;

            case NotifyCollectionChangedAction.Replace:
                var startIndex = e.NewStartingIndex;
                var endIndex = startIndex + e.NewItems!.Count;

                var oldItems = ViewModels.Skip(startIndex).Take(endIndex - startIndex).ToList();
                oldItems.ForEach(item => item.Dispose());
                for (var i = startIndex; i < endIndex; i++)
                {
                    ViewModels[i] = TViewModel.FromModel(Model[i]);
                }
                var newItems = ViewModels.Skip(startIndex).Take(endIndex - startIndex).ToList();
                RaiseReplace(startIndex, oldItems, newItems);
                break;

            case NotifyCollectionChangedAction.Move:
                // might be implemented later
                throw new NotImplementedException();

            case NotifyCollectionChangedAction.Reset:
                RebuildFromSource();
                RaiseReset();
                break;
        }
    }

    private void RebuildFromSource()
    {
        if (Model.Count == 0)
        {
            ViewModels.ForEach(item => item.Dispose());
            ViewModels.Clear();
            return;
        }

        var viewModelsByModels = ViewModels.ToDictionary(viewModel => viewModel.Model);
        ViewModels.Clear();

        for (var i = 0; i < Model.Count; i++)
        {
            var viewModel = viewModelsByModels.TryGetValue(Model[i], out var existingViewModel) ? existingViewModel : TViewModel.FromModel(Model[i]);
            viewModelsByModels.Remove(Model[i]);
            ViewModels.Add(viewModel);
        }

        var removedViewModels = viewModelsByModels.Values.ToList();
        removedViewModels.ForEach(item => item.Dispose());
    }

    // ------
    // Events
    // ------

    public void RaiseInsert(int index, IList<TViewModel> items)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items as IList, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseRemove(int index, IList<TViewModel> items)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items as IList, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseReplace(int index, IList<TViewModel> oldItems, IList<TViewModel> newItems)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (newItems as IList)!, (oldItems as IList)!, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseReset()
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        CollectionChanged?.Invoke(this, e);
    }

    // --------
    // Disposal
    // --------

    public void Dispose()
    {
        foreach (var viewModel in ViewModels)
        {
            viewModel.Dispose();
        }
        Model.CollectionChanged -= ApplyChanges;
    }
}
