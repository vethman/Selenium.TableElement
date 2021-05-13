# Selenium.TableElement
Having trouble testing tables on your frontend? Selenium.TableElement will make things easier for you. 

This package has the following features:
- Creation of header collection that functions as indexer for row columns
- Creation of column collection per row
- Get column WebElement by header name or index
- Supports tables with colspan / **!!!Does not supports tables with rowspan!!!**

## Extension on WebDriver and WebElement to get ITableElement in the given context
There are multiple ways to find your TableElement.

- First parameter represents the seperate headers, second parameter represents the rows and **optional** third parameter specifies the columns inside each row
```csharp
ITableElement tableElement = _webDriver.FindTableElement(By.XPath("//table/thead/tr/th"), By.XPath("//table/tbody/tr"), By.ClassName("divTableCell"));
```

- Same as above but first you find the headers and rows and pass them to FindTableElement, you can also pass the optional column selector
```csharp
var headers = _webDriver.FindElements(By.ClassName("divTableHead"));
var rows = _webDriver.FindElements(By.CssSelector(".divTableBody>.divTableRow"));
ITableElement tableElement = _webDriver.FindTableElement(headers, rows, By.ClassName("divTableCell"));
```

## Setup your own TableRowElement(s)
```csharp
var headers = _webDriver.FindElements(By.XPath("//table/thead/tr/th"));
var rows = _webDriver.FindElements(By.XPath("//table/tbody/tr"));
IEnumerable<ITableRowElement> tableRowElements = rows.Select(x => new TableRowElement(headers, x));
```

## Find column value in row
For example the following simple Html table that we want to test:

![image](https://user-images.githubusercontent.com/50708069/118183215-22ce9980-b43a-11eb-8a51-01671981eac9.png)

### Setup TableElement
```csharp
ITableElement tableElement = _webDriver.FindTableElement(By.XPath("//table/thead/tr/th"), By.XPath("//table/tbody/tr"));
```

### Get the second row by headername "First name"
```csharp
ITableRowElement tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("First name").Text == "Beta");
```
### Get the second row by index
```csharp
ITableRowElement tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn(0).Text == "Beta");
```

### Assert example by FluentAssertions
```csharp
tableRowElement.GetColumn("Last name").Text.Should().Be("Bit");
tableRowElement.GetColumn("Date of birth").Text.Should().Be("01-10-2002");
```

## Setup own implementation of TableRowElement
For example the following Html table with a button inside a column:

![image](https://user-images.githubusercontent.com/50708069/118184350-84dbce80-b43b-11eb-9be9-4ea90afb8183.png)

### Create seperate class
```csharp
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
```

### Integrate in your PageObject
```csharp
public IEnumerable<ButtonTableRow> ButtonTableRows
{
    get
    {
        var tableElement = _webDriver.FindTableElement(By.TagName("th"), By.CssSelector("tbody>tr"));
        return tableElement.TableRowElements.Select(x => new ButtonTableRow(x));
    }
}
```

### Use it in your test as follows
```csharp
var pageObject = new pageObject(_webDriver);

ButtonTableRow secondRow = pageObject.ButtonTableRows.Single(x => x.Rownumber == "Row 2");
secondRow.DeleteButton.Click();
```
