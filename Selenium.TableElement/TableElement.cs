using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium.TableElement
{
    public class TableElement : ITableElement
    {
        private readonly IDictionary<string, int> _headerIndexer;

        public IEnumerable<string> TableHeaderValues => _headerIndexer.Select(x => x.Key);
        public ReadOnlyCollection<ITableRowElement> TableRowElements { get; private set; }

        public TableElement(ISearchContext searchContext, By headersSelector, By rowsSelector) : this(searchContext, headersSelector, rowsSelector, By.XPath("./td"))
        {
        }

        public TableElement(ISearchContext searchContext, By headersSelector, By rowsSelector, By rowColumnSelector)
        {
            _headerIndexer = HeadersIncludingColspanAndDuplicate(searchContext.FindElements(headersSelector));

            TableRowElements = searchContext.FindElements(rowsSelector)
                .Select(x => (ITableRowElement)new TableRowElement(_headerIndexer, x, rowColumnSelector))
                .ToList()
                .AsReadOnly();
        }

        public TableElement(IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements) : this(headerElements, rowElements, By.XPath("./td"))
        {
        }

        public TableElement(IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements, By rowColumnSelector)
        {
            _headerIndexer = HeadersIncludingColspanAndDuplicate(headerElements);

            TableRowElements = rowElements
                .Select(x => (ITableRowElement)new TableRowElement(_headerIndexer, x, rowColumnSelector))
                .ToList()
                .AsReadOnly();
        }

        private IDictionary<string, int> HeadersIncludingColspanAndDuplicate(IEnumerable<IWebElement> headers)
        {
            if(headers.Any(x => !string.IsNullOrEmpty(x.GetAttribute("rowspan"))))
            {
                throw new NotSupportedException("TableHeader including rowspan not supported");
            }

            return headers
                .SelectMany(x => Enumerable.Range(0, Convert.ToInt32(x.GetAttribute("colspan") ?? "1")).Select(i => x.Text.Trim()))
                .Select((text, index) => new { text, index })
                .GroupBy(x => x.text)
                .SelectMany(x => x.Select((y, i) => new { text = y.text + (x.Count() > 1 ? "_" + (i + 1) : ""), y.index }))
                .ToDictionary(x => x.text, x => x.index);
        }
    }
}
