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

    // --------------------
    // IList implementation
    // --------------------

    public int Count => ViewModels.Count;

    bool IList.IsFixedSize => (ViewModels as IList).IsFixedSize;
    bool IList.IsReadOnly => (ViewModels as IList).IsReadOnly;
    bool ICollection.IsSynchronized => (ViewModels as ICollection).IsSynchronized;
    object ICollection.SyncRoot => (ViewModels as ICollection).SyncRoot;

    object? IList.this[int index]
    {
        get => ViewModels[index];
        set
        {
            if (value is not TViewModel itemViewModel)
                throw new InvalidCastException();

            Model[index] = itemViewModel.Model;
        }
    }

    int IList.Add(object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Model.Add(itemViewModel.Model);
        return Model.Count - 1;
    }

    void IList.Clear()
        => Model.Clear();

    bool IList.Contains(object? value)
        => value is TViewModel itemViewModel && Model.Contains(itemViewModel.Model);

    int IList.IndexOf(object? value)
        => value is TViewModel itemViewModel ? Model.IndexOf(itemViewModel.Model) : -1;

    void IList.Insert(int index, object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Model.Insert(index, itemViewModel.Model);
    }

    void IList.Remove(object? value)
    {
        if (value is not TViewModel itemViewModel)
            throw new InvalidCastException();

        Model.Remove(itemViewModel.Model);
    }

    void IList.RemoveAt(int index)
        => Model.RemoveAt(index);

    void ICollection.CopyTo(Array array, int index)
        => (ViewModels as ICollection).CopyTo(array, index);

    // ---------------
    // Synchronization
    // ---------------

    private void ApplyChanges(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                var itemsToAdd = e.NewItems!.OfType<TModel>().Select(TViewModel.FromModel).ToList();
                if (e.NewStartingIndex < 0)
                {
                    ViewModels.AddRange(itemsToAdd);
                    RaiseAdd(itemsToAdd);
                }
                else
                {
                    ViewModels.InsertRange(e.NewStartingIndex, itemsToAdd);
                    RaiseInsert(e.NewStartingIndex, itemsToAdd);
                }

                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.NewStartingIndex < 0)
                    goto case NotifyCollectionChangedAction.Reset; // can't figure out unindexed removal

                var removeCount = e.OldItems!.Count;
                var removedItems = ViewModels.Skip(e.NewStartingIndex).Take(removeCount).ToList();
                ViewModels.RemoveRange(e.NewStartingIndex, removeCount);
                RaiseRemove(e.NewStartingIndex, removedItems);
                break;

            case NotifyCollectionChangedAction.Replace:
                var startIndex = e.NewStartingIndex;
                var endIndex = startIndex + e.NewItems!.Count;

                var oldItems = ViewModels.Skip(startIndex).Take(endIndex - startIndex).ToList();
                for (var i = startIndex; i < endIndex; i++)
                {
                    ViewModels[i] = TViewModel.FromModel(Model[i]);
                }
                var newItems = ViewModels.Skip(startIndex).Take(endIndex - startIndex).ToList();

                break;

            case NotifyCollectionChangedAction.Reset:
                Synchronize();
                RaiseReset();
                break;

            default:
                throw new NotSupportedException($"Cannot process collection change action of type {e.Action}.");
        }
    }

    private void Synchronize()
    {
        if (Model.Count == 0)
            ViewModels = new List<TViewModel>();

        var startingIndex = 0;
        var minCount = Math.Min(Model.Count, ViewModels.Count);
        for (var i = 0; i < minCount; i++)
        {
            if (ViewModels[i].Model == Model[i])
                startingIndex++;
            else
                break;
        }

        var viewModelsByModels = ViewModels.Skip(startingIndex).ToDictionary(viewModel => viewModel.Model);
        if (ViewModels.Count > Model.Count)
            ViewModels.RemoveRange(Model.Count, ViewModels.Count - Model.Count);

        for (var i = startingIndex; i < Model.Count; i++)
        {
            var viewModel = viewModelsByModels.TryGetValue(Model[i], out var existingViewModel) ? existingViewModel : TViewModel.FromModel(Model[i]);

            if (i < ViewModels.Count)
                ViewModels[i] = viewModel;
            else
                ViewModels.Add(viewModel);
        }
    }

    // ------
    // Events
    // ------

    public void RaiseAdd(IList<TViewModel> items)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items as IList, ViewModels.Count - items.Count);
        CollectionChanged?.Invoke(this, e);
    }

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
        Model.CollectionChanged -= ApplyChanges;
    }
}
