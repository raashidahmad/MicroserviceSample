using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceSample.Orders.Models
{
    public class OrderModel
    {
        [Required]
        public string OrderedItem { get; set; }
        [Required]
        public string Currency { get; set; }
    }

    public class OrderItem
    {
        public string Item { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderView
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public string Dated { get; set; }
    }

    public class ActionResponse
    {
        public int ReturnedId { get; set; } = 1;
        public string Message { get; set; } = "";
        public bool Success { get; set; } = true;
    }
}
