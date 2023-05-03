using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using main_backend.Models;

namespace main_backend.Services{

    public class PostService {
        private readonly IMongoCollection<PostModel> _postCollection;

        public PostService(IOptions<MongoDBSettings> mongoDBSettings){
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _postCollection = database.GetCollection<PostModel>(mongoDBSettings.Value.PostsCollectionName);
        }
        
        public async Task<List<PostModel>> ListAllPostsAsync(string owner)=>
            await _postCollection.Find(x => x.Owner != owner && x.Status == "unfinish").ToListAsync();

        public async Task CreatePostAsync(string userId,string username,NewPostModel newPost){
            Random rnd = new Random();
            int index  = rnd.Next(0, 18);
            var post = new PostModel{
                Owner = userId,
                OwnerUserName = username,
                Limit = newPost.Limit,
                Time = newPost.Time,
                Status = "unfinish",
                ImgIndex = index,
                ImgOrderIndexList = new List<int>{}
            };
            await _postCollection.InsertOneAsync(post);
        }

    }
}