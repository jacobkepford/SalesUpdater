using System;

namespace SalesUpdater.Core
{
    public class Email
    {
        public string OrderNumber { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string OrderPerson { get; set; }
        public string EmailAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Subtotal { get; set; }
        public string Total { get; set; }
        public DateTime OrderDate { get; set; }




    }
}