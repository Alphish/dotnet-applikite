using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Alphicsh.Applikite.Models;

public class CollectionSource<TItem> : ICollectionSource<TItem>
    where TItem : class
{
    private List<TItem> UnderlyingList { get; } = new List<TItem>();

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    // -----------
    // Enumeration
    // -----------

    public IEnumerator<TItem> GetEnumerator()
        => UnderlyingList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => UnderlyingList.GetEnumerator();

    // ----------
    // Collection
    // ----------

    public int Count => UnderlyingList.Count;
    bool ICollection<TItem>.IsReadOnly => (UnderlyingList as ICollection<TItem>).IsReadOnly;

    public bool Contains(TItem item)
        => UnderlyingList.Contains(item);

    public void CopyTo(TItem[] array, int arrayIndex)
        => UnderlyingList.CopyTo(array, arrayIndex);

    public void Add(TItem item)
    {
        UnderlyingList.Add(item);
        RaiseAdd(item);
    }

    public void Clear()
    {
        if (UnderlyingList.Count == 0)
            return;

        UnderlyingList.Clear();
        RaiseReset();
    }

    public bool Remove(TItem item)
    {
        var index = IndexOf(item);
        if (index < 0)
            return false;

        RemoveAt(index);
        return true;
    }

    // ----
    // List
    // ----

    public TItem this[int index]
    {
        get => UnderlyingList[index];
        set
        {
            var oldItem = UnderlyingList[index];
            if (oldItem == value)
                return;

            UnderlyingList[index] = value;
            RaiseReplace(index, oldItem, value);
        }
    }

    public int IndexOf(TItem item)
        => UnderlyingList.IndexOf(item);

    public void Insert(int index, TItem item)
    {
        UnderlyingList.Insert(index, item);
        RaiseInsert(index, item);
    }

    public void RemoveAt(int index)
    {
        var item = UnderlyingList[index];
        UnderlyingList.RemoveAt(index);
        RaiseRemove(index, item);
    }

    // ------
    // Events
    // ------

    public void RaiseAdd(TItem item)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseInsert(int index, TItem item)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseRemove(int index, TItem item)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseReplace(int index, TItem oldItem, TItem newItem)
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
        CollectionChanged?.Invoke(this, e);
    }

    public void RaiseReset()
    {
        var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        CollectionChanged?.Invoke(this, e);
    }
}
