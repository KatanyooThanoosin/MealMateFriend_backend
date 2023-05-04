using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using main_backend.Models;

namespace main_backend.Services{

    public class OrderService {
        private readonly IMongoCollection<OrderModel> _orderCollection;

        public OrderService(IOptions<MongoDBSettings> mongoDBSettings){
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _orderCollection = database.GetCollection<OrderModel>(mongoDBSettings.Value.OrdersCollectionName);
        }

        public async Task<List<OrderModel>> ListOrdersByPostId(string postId)=>
            await _orderCollection.Find(x=> x.PostId == postId && (x.Status =="waiting"|| x.Status == "accept")).ToListAsync();

        public async Task<List<OrderModel>> ListOrdersByUserId(string userId)=>
            await _orderCollection.Find(x=> x.Owner == userId && (x.Status =="waiting"|| x.Status == "accept")).ToListAsync();
        
        public async Task<List<OrderModel>> ListAcceptOrderByPostIdAsync(string postId)=>
            await _orderCollection.Find(x=>x.PostId == postId && x.Status == "accept").ToListAsync();

        public async Task CreateOrderAsync(NewOrderModel newOrder,string owner){
            var order = new OrderModel{
                Owner = owner,
                PostId = newOrder.PostId,
                Foodname = newOrder.Foodname,
                Note = newOrder.Note,
                Count = newOrder.Count,
                Status = "waiting"
            };
            await _orderCollection.InsertOneAsync(order);
        }
    }
}