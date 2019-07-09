using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HelloMongoDB
{
    public class DataAccess
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<Product> collection;

        public DataAccess()
        {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("test");
            collection = database.GetCollection<Product>("products");
            if (collection.CountDocuments(new BsonDocument()) == 0)
                CreateData();
        }

        public void CreateData()
        {
            List<Product> list = new List<Product> {
                new Product
                {
                    Name = "Phone XL",
                    Price = 799,
                    Description = "A large phone with one of the best screens"
                },
                new Product
                {
                    Name = "Phone Mini",
                    Price = 699,
                    Description = "A great phone with one of the best cameras"
                },
                new Product
                {
                    Name = "Phone Standard",
                    Price = 299,
                    Description = ""
                }
            };
            collection.InsertMany(list);
        }

        public IEnumerable<Product> GetProducts()
        {
            return collection.Find(p => true).ToList();
        }

        public Product GetProduct(ObjectId id)
        {
            return collection.Find(p => p.Id == id).FirstOrDefault();
        }

        public Product Create(Product product)
        {
            collection.InsertOne(product);
            return product;
        }

        public void Update(ObjectId id, Product product)
        {
            collection.FindOneAndReplace(p => p.Id == id, product);
        }

        public void Remove(ObjectId id)
        {
            collection.FindOneAndDelete(p => p.Id == id);
        }
    }
}