using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium.TableElement
{
    public class TableRowElement : IWrapsElement
    {
        public readonly ReadOnlyCollection<IWebElement> _columns;
        private readonly IDictionary<string, int> _headerIndexer;

        public IWebElement WrappedElement { get; }

        public TableRowElement(ReadOnlyCollection<IWebElement> headers, IWebElement wrappedElement, By rowColumnSelector)
        {
            _columns = ColumnsIncludingColspan(wrappedElement.FindElements(rowColumnSelector));
            _headerIndexer = HeadersIncludingColspanAndDuplicate(headers);
            WrappedElement = wrappedElement;
        }

        public IWebElement GetColumn(string nameHeader)
        {
            if (!_headerIndexer.TryGetValue(nameHeader, out int result))
            {
                throw new NoSuchElementException($"Header '{nameHeader}' does not exist, available headers: {string.Join(Environment.NewLine, _headerIndexer.Select(x => x.Key))}");
            }

            return _columns[result];
        }

        public IWebElement GetColumn(int index)
        {
            return _columns[index];
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

        private ReadOnlyCollection<IWebElement> ColumnsIncludingColspan(ReadOnlyCollection<IWebElement> columns)
        {
            return columns
                .SelectMany(x =>
                {
                    var colspans = Enumerable.Range(0, Convert.ToInt32(x.GetAttribute("colspan") ?? "1"));
                    return colspans.Select(y => colspans.Count() == 1 || y == 0 ? x : null);
                }).ToList().AsReadOnly();
        }
    }
}
