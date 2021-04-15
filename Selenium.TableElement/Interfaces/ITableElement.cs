using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Selenium.TableElement.Interfaces
{
    public interface ITableElement
    {
        IEnumerable<string> TableHeaderValues { get; }
        ReadOnlyCollection<ITableRowElement> TableRowElements { get; }
    }
}