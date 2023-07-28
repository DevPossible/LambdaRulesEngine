using Xunit;
using App.Core.Helpers;

namespace App.Core.Tests
{
    public class ZipHelperTests
    {
        [Theory]
        [InlineData("35115", "*")]
        [InlineData("35115", "3*")]
        [InlineData("35115", "35*")]
        [InlineData("35115", "351*")]
        [InlineData("35115", "3511*")]
        [InlineData("35115", "35115")]

        [InlineData("35115", "35-351*")]
        [InlineData("35115", "35111-35222")]
        [InlineData("35115", "1-99999")]

        [InlineData("35115", "2*, 1 - 20000, 35112, 35114, 35115, 4*, 43211")]
        [InlineData("35115", "2*, 1-20000, 35112, 35114, 35115, 4*, 43211")]
        [InlineData("35115", "2*,1-20000,35112,35114,35115,4*,43211")]
        [InlineData("35115", "35115,*")]
        [InlineData("35115", "35115,")]
        public void ZipCodeShouldMatchPattern(string zipCode, string pattern)
        {
            var helper = new ZipHelper();
            Assert.True(helper.IsZipMatch(zipCode, pattern));
        }

        [Theory]
        [InlineData("35115", "4*")]
        [InlineData("35115", "45*")]
        [InlineData("35115", "451*")]
        [InlineData("35115", "4511*")]
        [InlineData("35115", "45115")]

        [InlineData("35115", "452-454*")]
        [InlineData("35115", "45111-45222")]

        [InlineData("35115", "2*,1-20000,35112,35114,35116,4*,43211")]
        [InlineData("35115", "35116,")]
        public void ZipCodeShouldNotMatchPattern(string zipCode, string pattern)
        {
            var helper = new ZipHelper();
            Assert.False(helper.IsZipMatch(zipCode, pattern));
        }

    }
}