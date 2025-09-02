namespace Alphicsh.Applikite.ViewModels.Collections;

public interface IItemViewModel<TModel, TViewModel> : IViewModel<TModel>
    where TModel : class
    where TViewModel : IItemViewModel<TModel, TViewModel>
{
    static abstract TViewModel FromModel(TModel model);
}
