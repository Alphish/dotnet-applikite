using System.Windows.Input;
using Alphicsh.Applikite.Demo.Model;
using Alphicsh.Applikite.ViewModels;
using Alphicsh.Applikite.ViewModels.Collections;
using Alphicsh.Applikite.ViewModels.Commands;

namespace Alphicsh.Applikite.Demo.ViewModel;

public class ToDoViewModel : BaseViewModel
{
    public ToDoViewModel(AppModel model)
    {
        ToDoList = RelayCollection<ItemModel, ItemViewModel>(model.ToDoList);

        AddItemCommand = Command.From(AddItem);
        RemoveItemCommand = Command.WithRequiredParameter<ItemViewModel>(RemoveItem);
    }

    public ICollectionViewModel<ItemModel, ItemViewModel> ToDoList { get; }

    public ICommand AddItemCommand { get; }
    private void AddItem()
    {
        ToDoList.Model.Add(new ItemModel());
    }

    public ICommand RemoveItemCommand { get; }
    private void RemoveItem(ItemViewModel itemViewModel)
    {
        ToDoList.Model.Remove(itemViewModel.Model);
    }
}
