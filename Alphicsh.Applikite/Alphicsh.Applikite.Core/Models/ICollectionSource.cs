using System.Collections.Generic;
using System.Collections.Specialized;

namespace Alphicsh.Applikite.Models;

public interface ICollectionSource<TItem> : IList<TItem>, INotifyCollectionChanged
{
}
