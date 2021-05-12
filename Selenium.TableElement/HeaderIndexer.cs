using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.TableElement
{
    internal class HeaderIndexer
    {
        public IDictionary<string, int> HeadersIncludingColspanAndDuplicate(IEnumerable<IWebElement> headers)
        {
            if (headers.Any(x => !string.IsNullOrEmpty(x.GetAttribute("rowspan"))))
            {
                throw new NotSupportedException("TableHeader including rowspan not supported");
            }

            return headers
                .SelectMany(x => Enumerable.Range(0, Convert.ToInt32(x.GetAttribute("colspan") ?? "1")).Select(i => x.Text.Trim()))
                .Select((text, index) => new { text, index })
                .GroupBy(x => x.text)
                .SelectMany(x => x.Select((y, i) => new { text = y.text + (x.Count() > 1 ? "_" + (i + 1) : ""), y.index }))
                .ToDictionary(x => x.text, x => x.index);
        }
    }
}
