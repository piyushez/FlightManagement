
using Common.Models;
using InventoryService.DBContext;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InventoryService.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private  InventoryDbContext _inventoryContext;

        private IBus _busControl;
        public InventoryRepository(InventoryDbContext context,IBus bus)
        {
            _inventoryContext = context;
            _busControl = bus;
            
        }
       
        public List<InventoryTbl> GetInventory()
        {
            try
            {
               
                var res = _inventoryContext.InventoryTbl.ToList();
                GetInventoryData(res);
                if (res==null && res.Count == 0)
                    throw new Exception("No Inventory exists");
                return res;
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public async void GetInventoryData(object data)
        {
            await _busControl.SendAsync<object>("Demo-5", data);
        }
        public int AddInventory(InventoryTbl tbl)
        {
            try
            {
                int result = -1;
                tbl.FlightNumber=string.IsNullOrEmpty(tbl.FlightNumber) ? "" : tbl.FlightNumber;
                var res = _inventoryContext.InventoryTbl.Where(x => x.FlightNumber.ToLower() ==tbl.FlightNumber.ToLower()  ).ToList();
                if (res.Count != 0)
                {
                    throw new Exception("Inventory for airline " + tbl.AirlineNo + " is alreday exists");
                }
                else
                {
                    _inventoryContext.InventoryTbl.Add(tbl);
                   result= _inventoryContext.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
      
        public int UpdateInventory(InventoryTbl inventory)
        {
            try
            {
                _inventoryContext.InventoryTbl.Update(inventory);
                _inventoryContext.Entry(inventory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                return _inventoryContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
       

        public List<InventoryTbl> GetAllFlightBasedUponPlaces(string fromplace, string toplace)
        {
            try
            {
                var res = _inventoryContext.InventoryTbl.Where(x => x.ToPlace.ToLower() == toplace.ToLower() && x.FromPlace.ToLower() == fromplace.ToLower()).ToList();
                if (res.Count == 0)
                    throw new Exception("No Flight exists");
                return res;
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public static void Publish(IModel model)
        {
            model.QueueDeclare("Demo-Inventory",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            int count = 0;
            var message = new { Name = "Priyanka", Message = $"Test Msg.Count:{count}" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            model.BasicPublish("", "Demo-Inventory", null, body);
        }
    }
}
