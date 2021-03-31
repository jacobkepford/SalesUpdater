using System;

namespace EmailApi.Data
{
    public class Email
    {
        public string OrderNumber { get; set; }
        public string Product { get; set; }
        public string Quantity { get; set; }
        public string OrderPerson { get; set; }
        public string EmailAddress { get; set; }
        public DateTime OrderDate { get; set; }

    }
}