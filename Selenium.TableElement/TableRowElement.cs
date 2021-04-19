using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium.TableElement
{
    public class TableRowElement : IWrapsElement, ITableRowElement
    {
        private readonly ReadOnlyCollection<IWebElement> _columns;
        private readonly IDictionary<string, int> _headerIndexer;

        public IWebElement WrappedElement { get; }

        public TableRowElement(IDictionary<string, int> headerIndexer, IWebElement rowElement, By rowColumnSelector)
        {
            _columns = ColumnsIncludingColspan(rowElement.FindElements(rowColumnSelector));
            _headerIndexer = headerIndexer;
            WrappedElement = rowElement;
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

        private ReadOnlyCollection<IWebElement> ColumnsIncludingColspan(ReadOnlyCollection<IWebElement> columns)
        {
            if (columns.Any(x => !string.IsNullOrEmpty(x.GetAttribute("rowspan"))))
            {
                throw new NotSupportedException("TableRow including rowspan not supported");
            }

            return columns
                .SelectMany(x =>
                {
                    var colspans = Enumerable.Range(0, Convert.ToInt32(x.GetAttribute("colspan") ?? "1"));
                    return colspans.Select(y => colspans.Count() == 1 || y == 0 ? x : null);
                }).ToList().AsReadOnly();
        }
    }
}
