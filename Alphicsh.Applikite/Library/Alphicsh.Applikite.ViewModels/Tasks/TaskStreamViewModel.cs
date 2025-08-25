using System.ComponentModel;
using System.Windows.Input;
using Alphicsh.Applikite.Tasks;
using Alphicsh.Applikite.Tasks.Progress;
using Alphicsh.Applikite.ViewModels.Commands;
using Alphicsh.Applikite.ViewModels.Properties;

namespace Alphicsh.Applikite.ViewModels.Tasks;

public class TaskStreamViewModel<TResult, TProgress> : IViewModel
    where TProgress : IProgressData<TProgress>
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ITaskStream<TResult, TProgress> Model { get; }

    public TaskStreamViewModel(ITaskStream<TResult, TProgress> model)
    {
        Model = model;
        ResultProperty = new RelayViewModelProperty<TResult>(this, nameof(Result), Model.ResultSource);
        ProgressProperty = new RelayViewModelProperty<TProgress>(this, nameof(Progress), Model.ProgressSource);
        SendTaskCommand = Command.From(Model.SendTask);
        CancelTaskCommand = Command.From(Model.Cancel);
    }

    public RelayViewModelProperty<TResult> ResultProperty { get; }
    public TResult Result { get => ResultProperty.Value; set => ResultProperty.Value = value; }

    public RelayViewModelProperty<TProgress> ProgressProperty { get; }
    public TProgress Progress { get => ProgressProperty.Value; set => ProgressProperty.Value = value; }

    public ICommand SendTaskCommand { get; }
    public ICommand CancelTaskCommand { get; }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        ResultProperty.Dispose();
    }
}
