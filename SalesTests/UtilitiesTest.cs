using Xunit;
using EmailApi.Utilities;

namespace SalesTests
{
    public class UtilitiesTest
    {
        [Fact]
        public void EmailSearchMatch()
        {
            string message = "order from Bailey Kepford";
            string orderPersonNameExpr = "order from ([a-zA-z]* [a-zA-Z?][a-zA-z]*)";
            string orderPerson = EmailUtilities.EmailSearch(message, orderPersonNameExpr);

            Assert.Equal("Bailey Kepford", orderPerson);
        }
    }
}