using OpenQA.Selenium;

namespace Selenium.TableElement
{
    public interface ITableRowElement
    {
        IWebElement WrappedElement { get; }

        IWebElement GetColumn(int index);
        IWebElement GetColumn(string nameHeader);
    }
}