
using AirlineService.DBContext;
using AirlineService.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineService.Repository
{
    public class AirlineRepository : IAirlineRepository
    {
        private AppDBContext _appDBContext;
        private readonly IBus _busControl;

        public AirlineRepository(AppDBContext appDbContext,IBus bus)
        {
            _appDBContext = appDbContext;
            _busControl = bus;
        }
        public List<Airline> GetAirlineList()
        {
            List<Airline> lstAirlline = new List<Airline>();
            try
            {

                GetInventories();
                lstAirlline = _appDBContext.Airlines?.ToList();

            }
            catch(Exception ex)
            {
                throw;
            }

            return lstAirlline;
        }

     
        public int SaveAirline([FromBody] Airline airline)
        {
            try
            {
                int res = -1;
                var data = _appDBContext.Airlines.Find(airline.AirlineNo);
                if (data == null)
                {
                    _appDBContext.Airlines.Add(airline);
                    res = _appDBContext.SaveChanges();

                }
                else
                {
                    throw new Exception("Airline details already exists");
                }
                return res; 
            }
            catch
            {
                throw;
            }
        }
        public int DeleteAirlineByNo(string airlineNo)
        {

            try
            {
                int res = -1;
                var data = _appDBContext.Airlines.Find(airlineNo);
                if (data != null)
                {
                    _appDBContext.Airlines.Remove(data);
                    res = _appDBContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Airline details not available");
                }
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Airline GetAirlineByNo(string airlineNo)
        {
            try
            {
                var data = _appDBContext.Airlines.Find(airlineNo);

                if (data == null)
                {
                    throw new Exception("Data Not Found");
                }

                return data;
            }catch(Exception ex)
            {
                throw;
            }
        }
       
        public int UpdateAirline(Airline airline)
        {
            try
            {
                int res = -1;
                var data = _appDBContext.Airlines.Find(airline.AirlineNo);

                if (data != null)
                {
                    _appDBContext.Airlines.Update(airline);
                    res = _appDBContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Data not found");
                }

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
       public async void GetInventories()
        {
            //List<InventoryTbl> data = new List<InventoryTbl>();
            var data = new Object();
            await _busControl.ReceiveAsync<object>("Demo-5", x => {
                data = x;
                Console.WriteLine(x);
            });
            // return data;
            Console.WriteLine(data);
        }
        public static void Consume(IModel model)
        {
            model.QueueDeclare("Demo-Inventory",
                durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(model);

            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var msg = Encoding.UTF8.GetString(body);
                for(int i = 0; i <= 5; i++)
                {
                    Console.WriteLine(msg);

                }

            };

            model.BasicConsume("Demo-Inventory", true, consumer);
            Console.WriteLine("Consumer Started");
           // Console.ReadLine();
        }
    }
}
