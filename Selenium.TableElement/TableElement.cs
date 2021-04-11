using OpenQA.Selenium;
using Selenium.TableElement.Interfaces;
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

        public TableElement(IWebDriver webDriver, By headersSelector, By rowsSelector) : this(webDriver, headersSelector, rowsSelector, By.XPath("./td"))
        {
        }

        public TableElement(IWebDriver webDriver, By headersSelector, By rowsSelector, By rowColumnSelector)
        {
            _headerIndexer = HeadersIncludingColspanAndDuplicate(webDriver.FindElements(headersSelector));

            TableRowElements = webDriver.FindElements(rowsSelector)
                .Select(x => (ITableRowElement)new TableRowElement(_headerIndexer, x, rowColumnSelector))
                .ToList()
                .AsReadOnly();
        }

        private IDictionary<string, int> HeadersIncludingColspanAndDuplicate(ReadOnlyCollection<IWebElement> headers)
        {
            return headers
                .SelectMany(x => Enumerable.Range(0, Convert.ToInt32(x.GetAttribute("colspan") ?? "1")).Select(i => x.Text.Trim()))
                .Select((text, index) => new { text, index })
                .GroupBy(x => x.text)
                .SelectMany(x => x.Select((y, i) => new { text = y.text + (x.Count() > 1 ? "_" + (i + 1) : ""), y.index }))
                .ToDictionary(x => x.text, x => x.index);
        }
    }
}
