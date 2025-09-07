using System.Collections;
using System.Collections.Specialized;
using Shouldly;

namespace Alphicsh.Applikite.Models.Tests;

public class CollectionSourceTests
{
    // General

    [Fact]
    public void ShouldAccessItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        SourceCollection[0].ShouldBe("lorem");
        SourceCollection[1].ShouldBe("ipsum");
        SourceCollection[2].ShouldBe("dolor");
    }

    [Fact]
    public void ShouldContainItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        SourceCollection.Contains("lorem").ShouldBeTrue();
        SourceCollection.Contains("ipsum").ShouldBeTrue();
        SourceCollection.Contains("dolor").ShouldBeTrue();
        SourceCollection.Contains("whatever").ShouldBeFalse();
    }

    [Fact]
    public void ShouldCopyToArray()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");

        var testArray = new[] { "1", "2", "3", "4", "5", "6", "7" };
        SourceCollection.CopyTo(testArray, 2);
        testArray.ShouldBe(new[] { "1", "2", "lorem", "ipsum", "dolor", "6", "7" }, ignoreOrder: false);
    }

    [Fact]
    public void ShouldEnumerate()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");

        var expectedItems = new Queue<string>(new[] { "lorem", "ipsum", "dolor" });
        foreach (var item in (SourceCollection as IEnumerable))
        {
            item.ShouldBe(expectedItems.Dequeue());
        }
    }

    [Fact]
    public void ShouldNotBeReadOnly()
    {
        GivenEmptyCollectionSource();
        (SourceCollection as ICollection<string>).IsReadOnly.ShouldBeFalse();
    }

    // Add

    [Fact]
    public void ShouldAddItemToEmptyCollection()
    {
        GivenEmptyCollectionSource();
        GivenItem("lorem");

        WhenItemAdded();
        ThenExpectCollectionItems("lorem");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(0);
        ThenExpectNewItems("lorem");
    }

    [Fact]
    public void ShouldAddItemToCollectionWithItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItem("sit");

        WhenItemAdded();
        ThenExpectCollectionItems("lorem", "ipsum", "dolor", "sit");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(3);
        ThenExpectNewItems("sit");
    }

    // Insert

    [Fact]
    public void ShouldInsertItemAtStart()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(0);
        GivenItem("whatever");

        WhenItemInserted();
        ThenExpectCollectionItems("whatever", "lorem", "ipsum", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(0);
        ThenExpectNewItems("whatever");
    }

    [Fact]
    public void ShouldInsertItemInMiddle()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(2);
        GivenItem("whatever");

        WhenItemInserted();
        ThenExpectCollectionItems("lorem", "ipsum", "whatever", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(2);
        ThenExpectNewItems("whatever");
    }

    [Fact]
    public void ShouldInsertItemAtEnd()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(3);
        GivenItem("whatever");

        WhenItemInserted();
        ThenExpectCollectionItems("lorem", "ipsum", "dolor", "whatever");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(3);
        ThenExpectNewItems("whatever");
    }

    // Set

    [Fact]
    public void ShouldSetAtFirstIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(0);
        GivenItem("whatever");

        WhenItemSet();
        ThenExpectCollectionItems("whatever", "ipsum", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("lorem");
        ThenExpectNewIndex(0);
        ThenExpectNewItems("whatever");
    }

    [Fact]
    public void ShouldSetAtMiddleIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(1);
        GivenItem("whatever");

        WhenItemSet();
        ThenExpectCollectionItems("lorem", "whatever", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("ipsum");
        ThenExpectNewIndex(1);
        ThenExpectNewItems("whatever");
    }

    [Fact]
    public void ShouldSetAtLastIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(2);
        GivenItem("whatever");

        WhenItemSet();
        ThenExpectCollectionItems("lorem", "ipsum", "whatever");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("dolor");
        ThenExpectNewIndex(2);
        ThenExpectNewItems("whatever");
    }

    [Fact]
    public void ShouldSetToSameItem()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(2);
        GivenItem("dolor");

        WhenItemSet();
        ThenExpectCollectionItems("lorem", "ipsum", "dolor");
        ThenExpectNoCollectionChangeEvent();
    }

    // Remove

    [Fact]
    public void ShouldRemoveStartItem()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItem("lorem");

        WhenItemRemoved();
        ThenExpectCollectionItems("ipsum", "dolor");
        ThenExpectSuccessfulRemoval();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("lorem");
    }

    [Fact]
    public void ShouldRemoveMiddleItem()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItem("ipsum");

        WhenItemRemoved();
        ThenExpectCollectionItems("lorem", "dolor");
        ThenExpectSuccessfulRemoval();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("ipsum");
    }

    [Fact]
    public void ShouldRemoveEndItem()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItem("dolor");

        WhenItemRemoved();
        ThenExpectCollectionItems("lorem", "ipsum");
        ThenExpectSuccessfulRemoval();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("dolor");
    }

    [Fact]
    public void ShouldNotRemoveMissingItem()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItem("sit");

        WhenItemRemoved();
        ThenExpectCollectionItems("lorem", "ipsum", "dolor");
        ThenExpectFailedRemoval();
        ThenExpectNoCollectionChangeEvent();
    }

    [Fact]
    public void ShouldRemoveFromFirstIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(0);

        WhenItemRemovedAtIndex();
        ThenExpectCollectionItems("ipsum", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("lorem");
    }

    [Fact]
    public void ShouldRemoveFromMiddleIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(1);

        WhenItemRemovedAtIndex();
        ThenExpectCollectionItems("lorem", "dolor");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("ipsum");
    }

    [Fact]
    public void ShouldRemoveFromLastIndex()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenIndex(2);

        WhenItemRemovedAtIndex();
        ThenExpectCollectionItems("lorem", "ipsum");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("dolor");
    }

    // Clear

    [Fact]
    public void ShouldClearListWithItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");

        WhenCleared();
        ThenExpectNoCollectionItems();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void ShouldClearEmptyList()
    {
        GivenEmptyCollectionSource();

        WhenCleared();
        ThenExpectNoCollectionItems();
        ThenExpectNoCollectionChangeEvent();
    }

    // Replace items

    [Fact]
    public void ShouldReplaceItemsWithOtherItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItems("aet", "cetera");

        WhenAllItemsReplaced();
        ThenExpectCollectionItems("aet", "cetera");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void ShouldReplaceItemsWithSameItems()
    {
        GivenCollectionSourceWithItems("lorem", "ipsum", "dolor");
        GivenItems("lorem", "ipsum", "dolor");

        WhenAllItemsReplaced();
        ThenExpectCollectionItems("lorem", "ipsum", "dolor");
        ThenExpectNoCollectionChangeEvent();
    }

    // -----
    // Setup
    // -----

    private CollectionSource<string> SourceCollection { get; set; } = default!;
    private int Index { get; set; }
    private string? Item { get; set; }
    private IReadOnlyCollection<string>? Items { get; set; }

    private bool? RemovalResult { get; set; }
    private object? ReportedSender { get; set; }
    private NotifyCollectionChangedEventArgs? ReportedChange { get; set; }

    // Given

    private void GivenEmptyCollectionSource()
    {
        SourceCollection = new CollectionSource<string>();
        SourceCollection.CollectionChanged += (sender, e) =>
        {
            if (ReportedSender != null || ReportedChange != null)
                Assert.Fail("Expected only one collection change throughout the test, but got multiple.");

            ReportedSender = sender;
            ReportedChange = e;
        };
    }

    private void GivenCollectionSourceWithItems(params string[] items)
    {
        SourceCollection = new CollectionSource<string>(items);
        SourceCollection.CollectionChanged += (sender, e) =>
        {
            if (ReportedSender != null || ReportedChange != null)
                Assert.Fail("Expected only one collection change throughout the test, but got multiple.");

            ReportedSender = sender;
            ReportedChange = e;
        };
    }

    private void GivenIndex(int index)
        => Index = index;

    private void GivenItem(string item)
        => Item = item;

    private void GivenItems(params string[] items)
        => Items = items;

    // When

    private void WhenItemAdded()
        => SourceCollection.Add(Item!);

    private void WhenItemInserted()
        => SourceCollection.Insert(Index, Item!);

    private void WhenItemSet()
        => SourceCollection[Index] = Item!;

    private void WhenItemRemoved()
        => RemovalResult = SourceCollection.Remove(Item!);

    private void WhenItemRemovedAtIndex()
        => SourceCollection.RemoveAt(Index);

    private void WhenCleared()
        => SourceCollection.Clear();

    private void WhenAllItemsReplaced()
        => SourceCollection.ReplaceItems(Items!);

    // Then

    private void ThenExpectNoCollectionItems()
    {
        SourceCollection.ShouldBeEmpty();
    }

    private void ThenExpectCollectionItems(params string[] items)
    {
        SourceCollection.Count.ShouldBe(items.Length);
        SourceCollection.ShouldBe(items, ignoreOrder: false);
    }

    private void ThenExpectSuccessfulRemoval()
        => RemovalResult.ShouldBe(true);

    private void ThenExpectFailedRemoval()
        => RemovalResult.ShouldBe(false);

    private void ThenExpectNoCollectionChangeEvent()
    {
        ReportedSender.ShouldBeNull();
        ReportedChange.ShouldBeNull();
    }

    private void ThenExpectCollectionChangeEvent()
    {
        ReportedSender.ShouldBe(SourceCollection);
        ReportedChange.ShouldNotBeNull();
    }

    private void ThenExpectChangeAction(NotifyCollectionChangedAction action)
        => ReportedChange!.Action.ShouldBe(action);

    private void ThenExpectOldIndex(int index)
        => ReportedChange!.OldStartingIndex.ShouldBe(index);

    private void ThenExpectOldItems(params string[] items)
    {
        var oldItems = ReportedChange!.OldItems;
        oldItems.ShouldNotBeNull();
        oldItems.Count.ShouldBe(items.Length);
        oldItems.OfType<string>().ShouldBe(items, ignoreOrder: false);
    }

    private void ThenExpectNewIndex(int index)
        => ReportedChange!.NewStartingIndex.ShouldBe(index);

    private void ThenExpectNewItems(params string[] items)
    {
        var newItems = ReportedChange!.NewItems;
        newItems.ShouldNotBeNull();
        newItems.Count.ShouldBe(items.Length);
        newItems.OfType<string>().ShouldBe(items, ignoreOrder: false);
    }
}
