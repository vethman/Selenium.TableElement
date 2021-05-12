using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium.TableElement
{
    /// <summary>
    /// TableElement supports testing Html Tables with Selenium.
    /// <list type="bullet">
    /// <item><description>All found headers to verify if headerselector gives correct headers</description></item>
    /// <item><description>Collection of <see cref="ITableRowElement"></description></item>
    /// </list>
    /// </summary>
    public class TableElement : ITableElement
    {
        private readonly IDictionary<string, int> _headerIndexer;

        /// <summary>
        /// All found headers, handy to verify if headerSelector/headerElements is correct
        /// </summary>
        public IEnumerable<string> TableHeaderValues => _headerIndexer.Select(x => x.Key);
        /// <summary>
        /// All TableRowElements found by rowSelector/rowElements
        /// </summary>
        public ReadOnlyCollection<ITableRowElement> TableRowElements { get; private set; }

        /// <summary>
        /// Creates TableElement that contains zero or multiple TableElementRows
        /// </summary>
        /// <param name="searchContext">Given context to find headers and rows</param>
        /// <param name="headersSelector">Selector to find IWebElements representing headercolumns</param>
        /// <param name="rowsSelector">Selector to find IWebElements representing tablerows</param>
        public TableElement(ISearchContext searchContext, By headersSelector, By rowsSelector) : this(searchContext, headersSelector, rowsSelector, By.XPath("./td"))
        {
        }

        /// <summary>
        /// Creates TableElement that contains zero or multiple TableElementRows
        /// </summary>
        /// <param name="searchContext">Given context to find headers and rows</param>
        /// <param name="headersSelector">Selector to find IWebElements representing headercolumns</param>
        /// <param name="rowsSelector">Selector to find IWebElements representing rows</param>
        /// <param name="rowColumnSelector">Selector to find IWebElements inside a row representing the columns</param>
        public TableElement(ISearchContext searchContext, By headersSelector, By rowsSelector, By rowColumnSelector)
        {
            _headerIndexer = new HeaderIndexer().HeadersIncludingColspanAndDuplicate(searchContext.FindElements(headersSelector));

            TableRowElements = searchContext.FindElements(rowsSelector)
                .Select(x => (ITableRowElement)new TableRowElement(_headerIndexer, x, rowColumnSelector))
                .ToList()
                .AsReadOnly();
        }

        /// <summary>
        /// Creates TableElement that contains zero or multiple TableElementRows
        /// </summary>
        /// <param name="headerElements">Collection of IWebElements representing headercolumns</param>
        /// <param name="rowElements">Collection of IWebElements representing rows</param>
        public TableElement(IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements) : this(headerElements, rowElements, By.XPath("./td"))
        {
        }

        /// <summary>
        /// Creates TableElement that contains zero or multiple TableElementRows
        /// </summary>
        /// <param name="headerElements">Collection of IWebElements representing headercolumns</param>
        /// <param name="rowElements">Collection of IWebElements representing rows</param>
        /// <param name="rowColumnSelector">Selector to find IWebElements inside a row representing the columns</param>
        public TableElement(IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements, By rowColumnSelector)
        {
            _headerIndexer = new HeaderIndexer().HeadersIncludingColspanAndDuplicate(headerElements);

            TableRowElements = rowElements
                .Select(x => (ITableRowElement)new TableRowElement(_headerIndexer, x, rowColumnSelector))
                .ToList()
                .AsReadOnly();
        }
    }
}
