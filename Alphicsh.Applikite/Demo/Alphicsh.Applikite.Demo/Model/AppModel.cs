using System;
using System.Threading;
using System.Threading.Tasks;
using Alphicsh.Applikite.Models;
using Alphicsh.Applikite.Tasks;
using Alphicsh.Applikite.Tasks.Channels;
using Alphicsh.Applikite.Tasks.Progress;
using Alphicsh.Applikite.Tasks.Sources;

namespace Alphicsh.Applikite.Demo.Model;

public class AppModel
{
    public ValueSource<string> TextSource { get; }
    public string Text { get => TextSource.Value; set => TextSource.Value = value; }

    public CollectionSource<ItemModel> ToDoList { get; }

    public ITaskStream<string, IntegerProgress> GenerateHashTaskStream { get; }

    public AppModel()
    {
        TextSource = ValueSource.Create("Hello!");
        ToDoList = new CollectionSource<ItemModel>()
        {
            new ItemModel { Name = "Lorem", Description = "The most basic element" },
            new ItemModel { Name = "Ipsum", Description = "A valuable support" },
        };

        var hashTaskSource = DelegateTaskSource.Of(GenerateHashAsync);
        var hashTaskChannel = new LastTaskChannel<string>("");
        GenerateHashTaskStream = new TaskStream<string, IntegerProgress>(hashTaskSource, hashTaskChannel);
    }

    public void UppercaseText()
        => Text = Text.ToUpperInvariant();

    public bool IsUppercaseApplicable()
        => Text != Text.ToUpperInvariant();

    private async Task<string> GenerateHashAsync(CancellationToken token, IProgress<object> progress)
    {
        var text = Text;
        var target = 300;
        for (var i = 0; i < target; i++)
        {
            await Task.Delay(10);
            token.ThrowIfCancellationRequested();
            progress.Report(new IntegerProgress { Current = i, Target = target });
        }
        token.ThrowIfCancellationRequested();
        return text.GetHashCode().ToString("X");
    }
}

