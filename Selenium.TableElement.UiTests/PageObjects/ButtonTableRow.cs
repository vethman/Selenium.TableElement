using OpenQA.Selenium;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class ButtonTableRow
    {
        private readonly ITableRowElement _tableRowElement;

        public string Rownumber => _tableRowElement.GetColumn("Rownumber").Text;
        public IWebElement DeleteButton => _tableRowElement.GetColumn("Delete?");

        public ButtonTableRow(ITableRowElement tableRowElement)
        {
            _tableRowElement = tableRowElement;
        }
    }
}
