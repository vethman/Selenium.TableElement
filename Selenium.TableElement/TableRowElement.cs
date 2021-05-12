using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Selenium.TableElement
{
    /// <summary>
    /// TableRowElement supports testing Html TableRows with Selenium.
    /// </summary>
    public class TableRowElement : IWrapsElement, ITableRowElement
    {
        private readonly ReadOnlyCollection<IWebElement> _columns;
        private readonly IDictionary<string, int> _headerIndexer;

        /// <summary>
        /// Gets the <see cref="IWebElement"/> wrapped by this object.
        /// </summary>
        public IWebElement WrappedElement { get; }

        /// <summary>
        /// Creates TableRowElement
        /// </summary>
        /// <param name="headerIndexer">Indexer that contains header values</param>
        /// <param name="rowElement">IWebelement that represents row</param>
        /// <param name="rowColumnSelector">Selector to find IWebElements inside a row representing the columns</param>
        public TableRowElement(IDictionary<string, int> headerIndexer, IWebElement rowElement, By rowColumnSelector)
        {
            _columns = ColumnsIncludingColspan(rowElement.FindElements(rowColumnSelector));
            _headerIndexer = headerIndexer;
            WrappedElement = rowElement;
        }

        /// <summary>
        /// Creates TableRowElement
        /// </summary>
        /// <param name="headerElements">Collection of IWebelements that represents headerColumns</param>
        /// <param name="rowElement">IWebelement that represents row</param>
        /// <param name="rowColumnSelector">Selector to find IWebElements inside a row representing the columns</param>
        public TableRowElement(IEnumerable<IWebElement> headerElements, IWebElement rowElement, By rowColumnSelector)
        {
            _columns = ColumnsIncludingColspan(rowElement.FindElements(rowColumnSelector));
            _headerIndexer = new HeaderIndexer().HeadersIncludingColspanAndDuplicate(headerElements);
            WrappedElement = rowElement;
        }

        /// <summary>
        /// Get a column of the tablerow.
        /// </summary>
        /// <param name="nameHeader">Header value to get corresponding column.</param>
        /// <returns>The matching <see cref="IWebElement"/> on the current context.</returns>
        public IWebElement GetColumn(string nameHeader)
        {
            if (!_headerIndexer.TryGetValue(nameHeader, out int result))
            {
                throw new NoSuchElementException($"Header '{nameHeader}' does not exist, available headers: {string.Join(Environment.NewLine, _headerIndexer.Select(x => x.Key))}");
            }

            return _columns[result];
        }

        /// <summary>
        /// Get a column of the tablerow.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The matching <see cref="IWebElement"/> on the current context.</returns>
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
