using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class SimpleTable
    {
        private readonly IWebDriver _webDriver;

        public ITableElement ColspanTableElement => _webDriver.FindTableElement(By.XPath("//table/thead/tr/th"), By.XPath("//table/tbody/tr"));
        public IWebElement TableParent => _webDriver.FindElement(By.TagName("table"));
        public ReadOnlyCollection<IWebElement> TableHeaders => _webDriver.FindElements(By.XPath("//table/thead/tr/th"));
        public ReadOnlyCollection<IWebElement> TableRows => _webDriver.FindElements(By.XPath("//table/tbody/tr"));

        public SimpleTable(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void Open()
        {
            _webDriver.Navigate().GoToUrl(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "DemoHtml/simpletable.html")).AbsoluteUri);
        }
    }
}
