using Xunit;
using SalesUpdater.Data;
using System.Text.RegularExpressions;

namespace SalesUpdater.Tests
{
    public class UtilitiesTest
    {

        [Theory]
        //Order Number
        [InlineData("Order: \\#([0-9]+)", "33477")]
        //Product
        [InlineData("QuantityPrice(.*)[\\d]{1,3}\\$", "RhinoHide Tractor Canopy - Removable")]
        //Quantity
        [InlineData("Price.*([\\d]{1,3})\\$", "1")]
        //Name
        [InlineData("order from ([a-zA-z]* [a-zA-Z]. ?[a-zA-z]*):", "Eric Weinmann")]
        //Order Date
        [InlineData("\\(([A-Z][a-z]+ [0-9]*, [0-9]{4})\\)Product", "June 8, 2021")]
        //Email address
        [InlineData("address.*[0-9]+([a-zA-Z].*@.*\\.[a-zA-Z]+)Shipping", "eweinmann@sbcglobal.net")]
        //Payment method
        [InlineData("method:(.*)Total", "PayPal")]
        //Subtotal
        [InlineData("Subtotal:(.*\\.\\d{2})Shipping", "$265.00")]
        //Total
        [InlineData("Total:(.*\\.\\d{2})Order", "$321.00")]
        public void EmailSearchMatch(string expr, string result)
        {
            // string message = "Ask Tractor MikeNew Order: #33477You’ve received the following order from Eric Weinmann:[Order #33477] (June 8, 2021)ProductQuantityPriceRhinoHide Tractor Canopy - Removable1$265.00Subtotal:$265.00Shipping:$56.00 via Shipping &amp; HandlingTax:$0.00Payment method:PayPalTotal:$321.00Order NotesThere are no notes for this order yet.Billing addressEric Weinmann5705 Fawnridge RdAuburn, CA 95602951-970-5271eweinmann@sbcglobal.netShipping addressBob Weinmann217 N 19th StreetCanon City, CO 81212Tractor Mike and his family thank you for your order…it’s a pleasure doing business with you!";
            // string orderPerson = EmailUtilities.EmailSearch(message, expr);
            // Assert.Equal(result, orderPerson);
        }

    }
}