using Alphicsh.Applikite.Models;

namespace Alphicsh.Applikite.Demo.Model;

public class ItemModel
{
    public ValueSource<string> NameSource { get; } = ValueSource.Create("Name");
    public string Name { get => NameSource.Value; set => NameSource.Value = value; }

    public ValueSource<string> DescriptionSource { get; } = ValueSource.Create("Enter description here...");
    public string Description { get => DescriptionSource.Value; set => DescriptionSource.Value = value; }
}
