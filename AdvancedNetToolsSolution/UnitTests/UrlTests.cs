using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UrlTests
    {
        [Theory]
        [InlineData("abv.bg")]
        [InlineData("http://aa.com")]
        [InlineData("https://aa.com")]
        public void TestForValidUrlTest(string uriName)
        {
            if (!uriName.Trim().StartsWith(value: "http")&& !uriName.Trim().StartsWith(value: "https")&& !uriName.Trim().StartsWith(value: "ftp"))
            {
                uriName = "http://" + uriName.Trim();
            }
            Uri uriResult;
            bool result = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            Assert.True(result);
        }
    }
}
