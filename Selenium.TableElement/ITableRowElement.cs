using OpenQA.Selenium;
using System.Collections.Generic;

namespace Selenium.TableElement
{
    /// <summary>
    /// Defines the interface through which the user can access tablerow columns.
    /// </summary>
    public interface ITableRowElement
    {
        /// <summary>
        /// Gets the <see cref="IWebElement"/> wrapped by this object.
        /// </summary>
        IWebElement WrappedElement { get; }
        /// <summary>
        /// All found headers, handy to verify if headerSelector/headerElements results in correct header values.
        /// </summary>
        IEnumerable<string> TableHeaderValues { get; }
        /// <summary>
        /// Get a column of the tablerow.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The matching <see cref="IWebElement"/> on the current context.</returns>
        IWebElement GetColumn(int index);
        /// <summary>
        /// Get a column of the tablerow.
        /// </summary>
        /// <param name="nameHeader">Header value to get corresponding column.</param>
        /// <returns>The matching <see cref="IWebElement"/> on the current context.</returns>
        IWebElement GetColumn(string nameHeader);
    }
}