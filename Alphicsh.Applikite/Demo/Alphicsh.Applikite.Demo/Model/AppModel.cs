using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.Demo.Model;

public class AppModel
{
    public ValueSource<string> TextSource { get; set; }
    public string Text { get => TextSource.Value; set => TextSource.Value = value; }

    public AppModel()
    {
        TextSource = ValueSource.Create("Hello!");
    }

    public void UppercaseText()
        => Text = Text.ToUpperInvariant();

    public bool IsUppercaseApplicable()
        => Text != Text.ToUpperInvariant();
}

