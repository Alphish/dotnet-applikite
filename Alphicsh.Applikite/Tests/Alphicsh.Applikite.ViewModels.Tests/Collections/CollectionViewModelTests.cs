using System.Collections;
using System.Collections.Specialized;
using Shouldly;

namespace Alphicsh.Applikite.ViewModels.Tests.Collections;

public class CollectionViewModelTests
{
    // General

    [Fact]
    public void ShouldAccessViewModelItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();

        ThenExpectViewModelToMatchModel();

        (ViewModel.Items as IList)[0].ShouldBeOfType<TestItemViewModel>();

        ViewModel.Items[0].UpperName.ShouldBe("APPLE");
        ViewModel.Items[0].UpperDescription.ShouldBe("ROUND AND TASTY");
        ViewModel.Items[1].UpperName.ShouldBe("GRAPEFRUIT");
        ViewModel.Items[1].UpperDescription.ShouldBe("BIG AND SOUR");
        ViewModel.Items[2].UpperName.ShouldBe("BANANA");
        ViewModel.Items[2].UpperDescription.ShouldBe("LONG AND SWEET");
    }

    [Fact]
    public void ShouldContainItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("apple");
        GivenNewItem("Kiwi", "Brown and green");

        ViewModel.Items.Contains(Items[0]).ShouldBeTrue(); // apple
        ViewModel.Items.Contains(Items[1]).ShouldBeFalse(); // kiwi

        ViewModel.Items.Contains(Items[0] as object).ShouldBeTrue(); // apple
        ViewModel.Items.Contains("whatever").ShouldBeFalse(); // ...
    }

    [Fact]
    public void ShouldGetIndexOfItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("banana");
        GivenExistingItem("apple");
        GivenNewItem("Kiwi", "Brown and green");

        ViewModel.Items.IndexOf(Items[0]).ShouldBe(2); // banana
        ViewModel.Items.IndexOf(Items[2]).ShouldBe(-1); // kiwi

        ViewModel.Items.IndexOf(Items[1] as object).ShouldBe(0); // apple
        ViewModel.Items.IndexOf("whatever").ShouldBe(-1); // ...
    }

    [Fact]
    public void ShouldCopyToArray()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();

        var viewModelsArray = new TestItemViewModel[] { null!, null!, null!, null!, null!, null!, null! };
        ViewModel.Items.CopyTo(viewModelsArray, 2);
        viewModelsArray.ShouldBe(new[] { null!, null!, ViewModel.Items[0], ViewModel.Items[1], ViewModel.Items[2], null!, null! }, ignoreOrder: false);

        var objectsArray = Array.CreateInstance(typeof(object), 7);
        for (var i = 0; i < 7; i++)
        {
            objectsArray.SetValue(i.ToString(), i);
        }
        ViewModel.Items.CopyTo(objectsArray, 2);
        objectsArray.OfType<object>().ShouldBe(new object[] { "0", "1", ViewModel.Items[0], ViewModel.Items[1], ViewModel.Items[2], "5", "6" }, ignoreOrder: false);
    }

    [Fact]
    public void ShouldNotAddInvalidItem()
    {
        GivenEmptyCollection();
        GivenViewModel();
        Action action = () => ViewModel.Items.Add("not a view model");
        action.ShouldThrow<InvalidCastException>();
    }

    [Fact]
    public void ShouldNotInsertInvalidItem()
    {
        GivenEmptyCollection();
        GivenViewModel();
        Action action = () => ViewModel.Items.Insert(0, "not a view model");
        action.ShouldThrow<InvalidCastException>();
    }

    [Fact]
    public void ShouldNotRemoveInvalidItem()
    {
        GivenEmptyCollection();
        GivenViewModel();
        Action action = () => ViewModel.Items.Remove("not a view model");
        action.ShouldThrow<InvalidCastException>();
    }

    [Fact]
    public void ShouldNotBeReadOnly()
    {
        GivenEmptyCollection();
        GivenViewModel();
        (ViewModel.Items as ICollection<TestItemViewModel>).IsReadOnly.ShouldBeFalse();
    }

    [Fact]
    public void ShouldNotHaveFixedSize()
    {
        GivenEmptyCollection();
        GivenViewModel();
        ViewModel.Items.IsFixedSize.ShouldBeFalse();
    }

    [Fact]
    public void ShouldHaveSynchronizationIGuess()
    {
        GivenEmptyCollection();
        GivenViewModel();
        (ViewModel.Items as ICollection).SyncRoot.ShouldNotBeNull();
        (ViewModel.Items as ICollection).IsSynchronized.ShouldBeOneOf(true, false);
    }

    // Add

    [Fact]
    public void ShouldAddItemToEmptyCollection()
    {
        GivenEmptyCollection();
        GivenViewModel();
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemAdded();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("kiwi");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(0);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldAddItemToCollectionWithItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemAdded();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "banana", "kiwi");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(3);
        ThenExpectNewItems("kiwi");
    }

    // Insert

    [Fact]
    public void ShouldInsertItemAtStart()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(0);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemInserted();
        ThenExpectViewModelToMatchModel();
        ThenExpectCollectionItems("kiwi", "apple", "grapefruit", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(0);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldInsertItemInMiddle()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(2);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemInserted();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "kiwi", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(2);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldInsertItemAtEnd()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(3);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemInserted();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "banana", "kiwi");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);
        ThenExpectNewIndex(3);
        ThenExpectNewItems("kiwi");
    }

    // Set

    [Fact]
    public void ShouldSetAtFirstIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(0);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemSet();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("kiwi", "grapefruit", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("apple");
        ThenExpectOldItemsDisposal();
        ThenExpectNewIndex(0);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldSetAtMiddleIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(1);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemSet();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "kiwi", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("grapefruit");
        ThenExpectOldItemsDisposal();
        ThenExpectNewIndex(1);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldSetAtLastIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(2);
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemSet();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "kiwi");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Replace);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("banana");
        ThenExpectOldItemsDisposal();
        ThenExpectNewIndex(2);
        ThenExpectNewItems("kiwi");
    }

    [Fact]
    public void ShouldSetToSameItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(2);
        GivenExistingItem("banana");

        WhenItemSet();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "banana");
        ThenExpectNoCollectionChangeEvent();
    }

    // Remove

    [Fact]
    public void ShouldRemoveStartItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("apple");

        WhenItemRemoved();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("grapefruit", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("apple");
        ThenExpectOldItemsDisposal();
    }

    [Fact]
    public void ShouldRemoveMiddleItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("grapefruit");

        WhenItemRemoved();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("grapefruit");
        ThenExpectOldItemsDisposal();
    }

    [Fact]
    public void ShouldRemoveEndItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("banana");

        WhenItemRemoved();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("banana");
        ThenExpectOldItemsDisposal();
    }

    [Fact]
    public void ShouldRemoveExistingItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("banana");

        WhenItemRemovedWithResult();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit");
        ThenExpectSuccessfulRemoval();
        ThenExpectCollectionChangeEvent();
    }

    [Fact]
    public void ShouldNotRemoveMissingItem()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenNewItem("Kiwi", "Brown and green");

        WhenItemRemovedWithResult();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "banana");
        ThenExpectFailedRemoval();
        ThenExpectNoCollectionChangeEvent();
    }

    [Fact]
    public void ShouldRemoveFromFirstIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(0);

        WhenItemRemovedAtIndex();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("grapefruit", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(0);
        ThenExpectOldItems("apple");
        ThenExpectOldItemsDisposal();
    }

    [Fact]
    public void ShouldRemoveFromMiddleIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(1);

        WhenItemRemovedAtIndex();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "banana");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(1);
        ThenExpectOldItems("grapefruit");
        ThenExpectOldItemsDisposal();
    }

    [Fact]
    public void ShouldRemoveFromLastIndex()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenIndex(2);

        WhenItemRemovedAtIndex();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Remove);
        ThenExpectOldIndex(2);
        ThenExpectOldItems("banana");
        ThenExpectOldItemsDisposal();
    }

    // Clear

    [Fact]
    public void ShouldClearListWithItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();

        WhenCleared();
        ThenExpectViewModelToMatchModel();

        ThenExpectNoCollectionItems();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Reset);
    }

    [Fact]
    public void ShouldClearEmptyList()
    {
        GivenEmptyCollection();
        GivenViewModel();

        WhenCleared();
        ThenExpectViewModelToMatchModel();

        ThenExpectNoCollectionItems();
        ThenExpectNoCollectionChangeEvent();
    }

    // Replace items

    [Fact]
    public void ShouldReplaceItemsWithOtherItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("Grapefruit");
        GivenNewItem("Kiwi", "Brown and green");

        var apple = ViewModel.Items[0];
        var grapefruit = ViewModel.Items[1];
        var banana = ViewModel.Items[2];

        WhenAllItemsReplaced();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("grapefruit", "kiwi");
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Reset);

        apple.IsDisposed.ShouldBeTrue();
        banana.IsDisposed.ShouldBeTrue();
        grapefruit.IsDisposed.ShouldBeFalse();
    }

    [Fact]
    public void ShouldReplaceItemsWithSameItems()
    {
        GivenInitialItem("Apple", "Round and tasty");
        GivenInitialItem("Grapefruit", "Big and sour");
        GivenInitialItem("Banana", "Long and sweet");
        GivenViewModel();
        GivenExistingItem("Apple");
        GivenExistingItem("Grapefruit");
        GivenExistingItem("Banana");

        WhenAllItemsReplaced();
        ThenExpectViewModelToMatchModel();

        ThenExpectCollectionItems("apple", "grapefruit", "banana");
        ThenExpectNoCollectionChangeEvent();
    }

    // Disposal

    [Fact]
    public void ShouldNotRespondToModelChangesAfterDisposal()
    {
        GivenEmptyCollection();
        GivenViewModel();

        Model.Items.Add(new TestItemModel { Name = "Kiwi", Description = "Brown and green" });
        ThenExpectViewModelToMatchModel();
        ThenExpectCollectionChangeEvent();
        ThenExpectChangeAction(NotifyCollectionChangedAction.Add);

        ReportedSender = null;
        ReportedChange = null;
        ViewModel.Dispose();

        Model.Items.Clear();
        Model.Items.ShouldBeEmpty();
        ViewModel.Items.ShouldNotBeEmpty();
        ViewModel.Items.ShouldAllBe(item => item.IsDisposed);
        ThenExpectNoCollectionChangeEvent();
    }

    // -----
    // Setup
    // -----

    private TestModel Model { get; set; } = new TestModel();
    private TestViewModel ViewModel { get; set; } = default!;
    private int Index { get; set; }
    private TestItemViewModel? Item { get; set; }
    private List<TestItemViewModel> Items { get; } = new List<TestItemViewModel>();

    private bool? RemovalResult { get; set; }
    private object? ReportedSender { get; set; }
    private NotifyCollectionChangedEventArgs? ReportedChange { get; set; }

    // Given

    private void GivenEmptyCollection()
    {
        // do nothing
    }

    private void GivenInitialItem(string name, string description)
        => Model.Items.Add(new TestItemModel { Name = name, Description = description });

    private void GivenViewModel()
    {
        ViewModel = new TestViewModel(Model);
        ViewModel.Items.CollectionChanged += (sender, e) =>
        {
            if (ReportedSender != null || ReportedChange != null)
                Assert.Fail("Expected only one collection change throughout the test, but got multiple.");

            ReportedSender = sender;
            ReportedChange = e;
        };
    }

    private void GivenIndex(int index)
        => Index = index;

    private void GivenNewItem(string name, string description)
    {
        var viewModel = new TestItemModel { Name = name, Description = description };
        Item = new TestItemViewModel(viewModel);
        Items.Add(Item);
    }

    private void GivenExistingItem(string name)
    {
        Item = ViewModel.Items.First(item => item.UpperName.Equals(name, StringComparison.OrdinalIgnoreCase));
        Items.Add(Item);
    }

    // When

    private void WhenItemAdded()
        => (ViewModel.Items as IList).Add(Item!);

    private void WhenItemInserted()
        => (ViewModel.Items as IList).Insert(Index, Item!);

    private void WhenItemSet()
        => (ViewModel.Items as IList)[Index] = Item!;

    private void WhenItemRemoved()
        => (ViewModel.Items as IList).Remove(Item!);

    private void WhenItemRemovedWithResult()
        => RemovalResult = ViewModel.Items.Remove(Item!);

    private void WhenItemRemovedAtIndex()
        => (ViewModel.Items as IList).RemoveAt(Index);

    private void WhenCleared()
        => (ViewModel.Items as IList).Clear();

    private void WhenAllItemsReplaced()
        => ViewModel.Items.ReplaceItems(Items);

    // Then

    private void ThenExpectNoCollectionItems()
    {
        ViewModel.Items.ShouldBeEmpty();
    }

    private void ThenExpectCollectionItems(params string[] expectedNames)
    {
        AssertViewModels(ViewModel.Items, expectedNames);
    }

    private void ThenExpectViewModelToMatchModel()
    {
        ViewModel.Items.Count.ShouldBe(Model.Items.Count);

        var viewModelModels = ViewModel.Items.Select(vm => vm.Model).ToList();
        Model.Items.ShouldBe(viewModelModels, ignoreOrder: false);
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
        ReportedSender.ShouldBe(ViewModel.Items);
        ReportedChange.ShouldNotBeNull();
    }

    private void ThenExpectChangeAction(NotifyCollectionChangedAction action)
        => ReportedChange!.Action.ShouldBe(action);

    private void ThenExpectOldIndex(int index)
        => ReportedChange!.OldStartingIndex.ShouldBe(index);

    private void ThenExpectOldItems(params string[] expectedNames)
    {
        var oldItems = ReportedChange!.OldItems;
        oldItems.ShouldNotBeNull();
        AssertViewModels(oldItems, expectedNames);
    }

    private void ThenExpectOldItemsDisposal()
    {
        var oldItems = ReportedChange!.OldItems!.OfType<TestItemViewModel>();
        oldItems.ShouldAllBe(item => item.IsDisposed);
    }

    private void ThenExpectNewIndex(int index)
        => ReportedChange!.NewStartingIndex.ShouldBe(index);

    private void ThenExpectNewItems(params string[] expectedNames)
    {
        var newItems = ReportedChange!.NewItems;
        newItems.ShouldNotBeNull();
        AssertViewModels(newItems, expectedNames);
    }

    private void AssertViewModels(IList viewModels, IReadOnlyCollection<string> expectedNames)
    {
        viewModels.Count.ShouldBe(expectedNames.Count);

        var actualNames = viewModels.OfType<TestItemViewModel>().Select(vm => vm.UpperName).ToList();
        actualNames.ShouldBe(expectedNames, StringComparer.OrdinalIgnoreCase, ignoreOrder: false);
    }
}
