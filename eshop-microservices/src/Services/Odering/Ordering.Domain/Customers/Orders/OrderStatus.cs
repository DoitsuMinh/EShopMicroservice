using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Customers.Orders;

public enum OrderStatus: int
{
    [Display(Description = "Placed", Name = "Placed")]
    Placed = 0,
    [Display(Description = "In realization", Name = "InRealization")]
    InRealization = 1,
    [Display(Description = "Canceled", Name = "Canceled")]
    Canceled = 2,
    [Display(Description = "Delivered", Name = "Delivered")]
    Delivered = 3,
    [Display(Description = "Sent", Name = "Sent")]
    Sent = 4,
    [Display(Description = "Waiting for payment", Name = "WaitingForPayment")]
    WaitingForPayment = 5

}

//public enum OrderStatus
//{
//    [Display(Description = "Cancelled", Name = "Cancelled")]
//    Cancelled,
//    [Display(Description = "Closed", Name = "Closed")]
//    Closed,
//    [Display(Description = "Awaiting Payment", Name = "Awaiting Payment")]
//    AwaitingPayment,
//    [Display(Description = "Processed", Name = "Processed")]
//    Processed,
//    [Display(Description = "Part Invoiced", Name = "part-invoiced")]
//    PartInvoiced,
//    [Display(Description = "Full Invoiced", Name = "full-invoiced")]
//    FullInvoiced,
//    [Display(Description = "NA", Name = "NA")]
//    NA
//}
