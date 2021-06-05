using Xunit;
using SalesUpdater.Data.Utilities;
using System.Text.RegularExpressions;

namespace SalesTests
{
    public class UtilitiesTest
    {

        [Theory]
        //Order Number
        [InlineData("Order: \\#([0-9]+)", "32258")]
        //Product
        [InlineData("Price(.*) [\\d]* \\$", "Tractor Loader Quick Attach Replacement Kubota LA950A, LA1001, LA1002,LA1150A, BF800, BF900, BF1100, LA950, LA1100 & LA1150 (#QA-KU5)")]
        //Quantity
        [InlineData("Price.* ([\\d]*) \\$", "1")]
        //Name
        [InlineData("order from ([a-zA-z]* [a-zA-Z]. ?[a-zA-z]*):", "Richard Seger")]
        //Order Date
        [InlineData("\\(([A-Z][a-z]+[0-9]*, [0-9]{4})\\)Product", "March20, 2021")]
        //Email address
        [InlineData("[0-9]{10}>(.*@.*\\.com)", "pnr149@yahoo.com")]
        //Payment method
        [InlineData("method: (.*)Total", "Credit Card")]
        //Subtotal
        [InlineData("Subtotal: (\\$[0-9]*,?[0-9]*?\\.[0-9]{2})Discount", "$899.00")]
        //Total
        [InlineData("Total: (\\$[0-9]*,?[0-9]*?\\.[0-9]{2})Order", "$1,078.76")]
        public void EmailSearchMatch(string expr, string result)
        {
            string message = "New Order: #32258You’ve received the following order from Richard Seger:[Order #32258]<http://track.smtpsendemail.com/9037916/c?p=F_sOud9wG23ZzA0j8Ylq_BB7vhhFf4-XzrWl_278dqa6-o97mphUPLZ808GS0mdeWYKaTOoY6xMvW7rtFS-AOLMn2pwnwfVZziKYHrPmnsoXTG3DXFtTKH-LvlwPMA-LtxVzPrBqvvwUJ05yH8gQqkkLdMPaLJ6ydbHmpwLZrFoA4spVyBvipc-3wIx0cZe->(March20, 2021)ProductQuantityPriceTractor Loader Quick Attach Replacement Kubota LA950A, LA1001, LA1002,LA1150A, BF800, BF900, BF1100, LA950, LA1100 & LA1150 (#QA-KU5) 1 $899.00Subtotal: $899.00Discount: -$20.21Shipping: $199.97 via Shipping & HandlingTax: $0.00Payment method: Credit CardTotal: $1,078.76Order Notes   - There are no notes for this order yet.Billing addressRichard Seger62910 County Line RDThree Rivers, MI 49093269-816-2559 <2698162559>pnr149@yahoo.com Shipping addressRichard Seger62910 County Line RDThree Rivers, MI 49093";
            string orderPerson = EmailUtilities.EmailSearch(message, expr);
            Assert.Equal(result, orderPerson);
        }

    }
}