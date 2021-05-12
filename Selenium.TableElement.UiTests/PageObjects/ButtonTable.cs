using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class ButtonTable
    {
        private readonly IWebDriver _webDriver;

        public IEnumerable<ButtonTableRow> buttonTableRows
        {
            get
            {
                var tableElement = _webDriver.FindTableElement(By.TagName("th"), By.CssSelector("tbody>tr"));
                return tableElement.TableRowElements.Select(x => new ButtonTableRow(x));
            }
        }

        public ButtonTable(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void Open()
        {
            _webDriver.Navigate().GoToUrl(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "DemoHtml/buttontable.html")).AbsoluteUri);
        }
    }
}
