using Xunit;
using SalesUpdater.Data.Utilities;
using System.Text.RegularExpressions;

namespace SalesTests
{
    public class UtilitiesTest
    {
        [Fact]
        public void EmailSearchMatch()
        {
            string message = "New Order: #32258You’ve received the following order from Richard Seger:[Order #32258]<http://track.smtpsendemail.com/9037916/c?p=F_sOud9wG23ZzA0j8Ylq_BB7vhhFf4-XzrWl_278dqa6-o97mphUPLZ808GS0mdeWYKaTOoY6xMvW7rtFS-AOLMn2pwnwfVZziKYHrPmnsoXTG3DXFtTKH-LvlwPMA-LtxVzPrBqvvwUJ05yH8gQqkkLdMPaLJ6ydbHmpwLZrFoA4spVyBvipc-3wIx0cZe->(March20, 2021)ProductQuantityPriceTractor Loader Quick Attach Replacement Kubota LA950A, LA1001, LA1002,LA1150A, BF800, BF900, BF1100, LA950, LA1100 & LA1150 (#QA-KU5) 1 $899.00Subtotal: $899.00Discount: -$20.21Shipping: $199.97 via Shipping & HandlingTax: $0.00Payment method: Credit CardTotal: $1,078.76Order Notes   - There are no notes for this order yet.Billing addressRichard Seger62910 County Line RDThree Rivers, MI 49093269-816-2559 <2698162559>pnr149@yahoo.com Shipping addressRichard Seger62910 County Line RDThree Rivers, MI 49093";
            string orderPersonNameExpr = "order from ([a-zA-z]* [a-zA-Z?][a-zA-z]*)";
            string orderPerson = EmailUtilities.EmailSearch(message, orderPersonNameExpr);

            Assert.Equal("Richard Seger", orderPerson);
        }

    }
}