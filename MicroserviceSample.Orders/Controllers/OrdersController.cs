using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroserviceSample.Orders.Models;
using MicroserviceSample.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MicroserviceSample.Orders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        IOrderService service;
        public OrdersController(IOrderService srvc)
        {
            service = srvc;
        }

        [HttpGet]
        [Route("GetItems")]
        public IActionResult GetItems()
        {
            return Ok(service.GetItems());
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(service.GetOrders());
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(service.AddNewOrder(model));
        }
    }
}
