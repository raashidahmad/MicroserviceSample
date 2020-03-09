using MicroserviceSample.Orders.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace MicroserviceSample.Orders.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Gets list of menu items
        /// </summary>
        /// <returns></returns>
        IEnumerable<OrderItem> GetItems();

        /// <summary>
        /// Gets list of all orders
        /// </summary>
        /// <returns></returns>
        IEnumerable<OrderView> GetOrders();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ActionResponse AddNewOrder(OrderModel model);
    }

    public class OrderService
    {
        List<OrderItem> itemsList;
        string fileName = "orders.txt";
        string ordersFilePath = "";
        public OrderService(IHostEnvironment env)
        {
            itemsList = new List<OrderItem>() 
            {
                new OrderItem { Item = "Chicken Combo", Price = 5.99m },
                new OrderItem { Item = "Doublicious Sandwich – Combo", Price = 4.99m },
                new OrderItem { Item = "Mashed Potatoes", Price = 2.50m },
                new OrderItem { Item = "Tenders Meal", Price = 19.99m },
                new OrderItem { Item = "Hot Wings", Price = 2.0m },
                new OrderItem { Item = "Soft Drink", Price = 1.99m },
            };
            ordersFilePath = env.ContentRootPath + "/" + fileName;
        }

        public IEnumerable<OrderItem> GetItems()
        {
            return itemsList;
        }

        public IEnumerable<OrderView> GetOrders()
        {
            return this.ReadOrdersFromFile();
        }

        public ActionResponse AddNewOrder(OrderModel model)
        {
            ActionResponse response = new ActionResponse();
            try
            {
                var ordersList = this.ReadOrdersFromFile();
                decimal price = (from item in itemsList
                                 where item.Item.Equals(model.OrderedItem, StringComparison.OrdinalIgnoreCase)
                                 select item.Price).FirstOrDefault();
                
                ordersList.Add(new OrderView()
                {
                    Id = (ordersList.Count + 1),
                    Item = model.OrderedItem,
                    Currency = model.Currency,
                    Price = price,
                    Dated = DateTime.Today.ToString()
                });
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        private List<OrderView> ReadOrdersFromFile()
        {
            List<OrderView> ordersList = new List<OrderView>();
            try
            {
                if (!File.Exists(ordersFilePath))
                {
                    File.Create(ordersFilePath);
                    FileIOPermission fp = new FileIOPermission(FileIOPermissionAccess.AllAccess, ordersFilePath);
                }
                string ordersText = File.ReadAllText(ordersFilePath);
                if (ordersText.Length > 0)
                {
                    ordersList = JsonConvert.DeserializeObject<List<OrderView>>(ordersText);
                }
            }
            catch(Exception)
            {
            }
            return ordersList;
        }

    }
}
