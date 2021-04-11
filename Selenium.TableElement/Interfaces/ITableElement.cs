using System.Collections.ObjectModel;

namespace Selenium.TableElement.Interfaces
{
    public interface ITableElement
    {
        ReadOnlyCollection<ITableRowElement> TableRowElements { get; }
    }
}