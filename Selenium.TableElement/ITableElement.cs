using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Selenium.TableElement
{
    /// <summary>
    /// Defines the interface through which the user can access table header values and table rows.
    /// </summary>
    public interface ITableElement
    {
        /// <summary>
        /// All found headers, handy to verify if headerSelector/headerElements is correct
        /// </summary>
        IEnumerable<string> TableHeaderValues { get; }
        /// <summary>
        /// All TableRowElements found by rowSelector/rowElements
        /// </summary>
        ReadOnlyCollection<ITableRowElement> TableRowElements { get; }
    }
}